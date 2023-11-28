using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

public class ObjectHandler : MonoBehaviour
{
    [SerializeField] private Image[] uiSprites;

    [SerializeField] private Button deleteButton;
    [SerializeField] private Button viewModelButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button regenButton;

    [SerializeField] private AudioSource selectFurnitureAudioSource;
    [SerializeField] private AudioSource uiButtonAudioSource;
    [SerializeField] private AudioSource dropFurnitureAudioSource;
    [SerializeField] private AudioSource errorAudio;

    [SerializeField] private TMP_Dropdown CostDisplayText;

    // state for user dragging ui icons into ar view
    private bool isDragging = false;
    // state for user moving ar objects
    private bool isMoveMode = false;
    // state for user rotating ar object
    private bool isRotateMode = false;
    private Image selectedSprite;
    private class FurnitureObject
    {
        public GameObject furnModel;
        public FurnitureData furnData;
    }
    private FurnitureObject selectedFurniture;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<FurnitureObject> instantiatedFurniture = new List<FurnitureObject>();
    private Vector3 objPositionBeforeMove;
    // for rotation
    private Quaternion objRotationBefore;
    private Vector2 initialFingerPosition;

    private string selectedVibe;
    private float totalCost = 0f;

    // array to hold available prefab options
    // we will store all of the indices of the prefabs to the db and use that to retrieve the correct
    // prefab for the piece of furniture
    public GameObject[] furniturePrefabs;
    public Sprite[] furnitureSprites;

    [SerializeField] TMP_Text debugText;

    [System.Serializable]
    public class FurnitureData
    {
        public int FUR_ID { get; set; }
        public string FUR_NAME { get; set; }
        public string FUR_LINK { get; set; }
        public string FUR_DESC { get; set; }
        public float FUR_COST { get; set; }
        public float FUR_DIM_L { get; set; }
        public float FUR_DIM_W { get; set; }
        public float FUR_DIM_H { get; set; }
        public string FUR_TYPE { get; set; }
    }

    private List<FurnitureData> furnitureData;
    private FurnitureData[] sofas;
    private FurnitureData[] chairs;
    private FurnitureData[] lamps;
    private FurnitureData[] tables;
    private FurnitureData[] desks;
    private FurnitureData[] drawers;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TMP_Text loadingPanelText;
    [SerializeField] private GameObject loadingPanelIcon;
    private bool isLoading = true;
    [SerializeField] private GameObject updateDisplay;

    public TMP_Text dropdownLabel;


    // info panel references
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text infoName;
    [SerializeField] private TMP_Text infoDesc;
    [SerializeField] private TMP_Text infoDims;
    [SerializeField] private TMP_Text infoCost;
    [SerializeField] private Image infoImage;
    private string productUrl;
    private bool showingInfoPanel = false;
    // favorite button references
    [SerializeField] private Image favoriteButtonImage;
    [SerializeField] private Sprite filledHeart;
    [SerializeField] private Sprite unfilledHeart;
    private int[] favFurnIds;


    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        foreach (Image sprite in uiSprites)
        {
            sprite.gameObject.SetActive(false);
        }
        deleteButton.gameObject.SetActive(false);
        viewModelButton.gameObject.SetActive(false);
        moveButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        regenButton.gameObject.SetActive(false);

        loadingPanelText.text = "Waiting for data...";
        loadingPanel.SetActive(true);
        updateDisplay.SetActive(false);
        updateDisplay.transform.localPosition = new Vector3(9999f, 0f, 0f);

        infoPanel.SetActive(false);
        infoPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);

        // parse favorites file into array
        LoadFavoriteFurnitureIds();
        // debugText.text = string.Join(", ", favFurnIds.Select(id => id.ToString()).ToArray());

        // retrive stored vibe or if not set, vibeError
        selectedVibe = PlayerPrefs.GetString("Vibe", "VibeERROR");
        StartCoroutine(GetFurnitureData(selectedVibe));
    }

    private void Start()
    {
        CostDisplayText.onValueChanged.AddListener(delegate
        {
            // Always update the selected text to the placeholder
            DropdownValueChanged();
        });
        // call initially to set drop down label
        DropdownValueChanged();
    }

    private IEnumerator GetFurnitureData(string vibeName)
    {
        int retryCount = 0;
        do
        {
            string apiUrl = "https://hammy-exchanges.000webhostapp.com/index.php?vibe_name=" + vibeName;
            // alternate endpoint to test on retry, finishes with another try on main endpoint
            if (retryCount == 1)
            {
                apiUrl = "http://hammy-exchanges.000webhostapp.com/index.php?vibe_name=" + vibeName;
                loadingPanelText.text = "Sorry, couldn't get the data. Retrying...";
            }
            using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    loadingPanel.SetActive(true);
                    Debug.LogError(www.error);
                    // wait 3 seconds before retrying
                    yield return new WaitForSeconds(3);
                    retryCount++;
                }
                else
                {
                    // parse JSON and store data
                    string json = www.downloadHandler.text;
                    try
                    {
                        furnitureData = JsonConvert.DeserializeObject<List<FurnitureData>>(json);
                        sofas = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Sofa" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();
                        chairs = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Chair" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();
                        lamps = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Lamp" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();
                        tables = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Table" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();
                        desks = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Desk" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();
                        drawers = furnitureData.Where(furniture => (furniture.FUR_TYPE == "Drawer" && furniture.FUR_ID <= furniturePrefabs.Length)).ToArray();

                        loadingPanel.SetActive(false);
                        isLoading = false;

                        // check that all FUR_ID are within the bounds of the prefab array
                        foreach (var furnitureItem in furnitureData)
                        {
                            if (furnitureItem.FUR_ID <= furniturePrefabs.Length)
                            {
                                updateDisplay.SetActive(true);
                                updateDisplay.transform.localPosition = Vector3.zero;
                            }
                        }

                        yield break;
                    }
                    catch (Exception e)
                    {
                        debugText.text = e.Message;
                    }

                }
            }
        } while (retryCount < 4);

        // if reached here, all retry attempts failed
        loadingPanelText.text = "Failed to retrieve data after multiple attempts";
        loadingPanelIcon.SetActive(false);
    }

    private void UpdateDropdown()
    {
        // Clear existing options in the dropdown
        CostDisplayText.ClearOptions();

        // Create a list of dropdown options (furniture items and their costs)
        List<string> options = new List<string>();
        foreach (FurnitureObject furniture in instantiatedFurniture)
        {
            options.Add($"{furniture.furnData.FUR_NAME}: ${furniture.furnData.FUR_COST:F2}");
        }
        // Update the dropdown options
        CostDisplayText.AddOptions(options);

        foreach (var item in CostDisplayText.options)
        {
            item.text = $"<size=50>{item.text}</size>"; // Adjust the font size (e.g., 20)
        }

        dropdownLabel.text = $"Total Cost: ${totalCost:F2}";
    }

    void DropdownValueChanged()
    {
        dropdownLabel.text = $"Total Cost: ${totalCost:F2}";
    }

    private void Update()
    {
        // if a furn obj is select, move ui elements to follow on each frame update
        if (selectedFurniture?.furnModel != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedFurniture.furnModel.transform.position);

            deleteButton.transform.position = screenPos + new Vector3(-300, 300, 0);
            regenButton.transform.position = screenPos + new Vector3(-150, 300, 0);
            viewModelButton.transform.position = screenPos + new Vector3(300, 300, 0);
            moveButton.transform.position = screenPos + new Vector3(-100, -150, 0);
            rotateButton.transform.position = screenPos + new Vector3(100, -150, 0);
        }
        else
        {
            // not object selected, move ui off screen 
            Vector3 offScreenPosition = new Vector3(-1000, -1000, 0);

            deleteButton.transform.position = offScreenPosition;
            viewModelButton.transform.position = offScreenPosition;
            moveButton.transform.position = offScreenPosition;
            rotateButton.transform.position = offScreenPosition;
            regenButton.transform.position = offScreenPosition;
        }
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
        EnhancedTouch.Touch.onFingerMove += FingerMove;
        EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
        EnhancedTouch.Touch.onFingerMove -= FingerMove;
        EnhancedTouch.Touch.onFingerUp -= FingerUp;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (isDragging || isLoading || showingInfoPanel) return;

        // selecting a default drag sprite
        foreach (Image uiSprite in uiSprites)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(uiSprite.rectTransform, finger.screenPosition))
            {
                isDragging = true;
                selectedSprite = uiSprite;
                uiSprite.transform.position = finger.screenPosition;
                uiSprite.gameObject.SetActive(true);
                break;
            }
        }
        // selecting a ui button
        foreach (Button button in new Button[] { deleteButton, viewModelButton, moveButton, rotateButton, regenButton })
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), finger.screenPosition))
            {
                if (uiButtonAudioSource != null)
                {
                    uiButtonAudioSource.Play();
                }
                if (button == deleteButton)
                {
                    DeleteObject();
                }
                else if (button == moveButton)
                {
                    isMoveMode = true;
                    objPositionBeforeMove = selectedFurniture.furnModel.transform.position;
                    // hide buttons in move mode
                    deleteButton.gameObject.SetActive(false);
                    viewModelButton.gameObject.SetActive(false);
                    rotateButton.gameObject.SetActive(false);
                    regenButton.gameObject.SetActive(false);
                }
                else if (button == rotateButton)
                {
                    isRotateMode = true;
                    objRotationBefore = selectedFurniture.furnModel.transform.rotation;
                    initialFingerPosition = finger.screenPosition;
                    // hide buttons in rotate mode
                    deleteButton.gameObject.SetActive(false);
                    viewModelButton.gameObject.SetActive(false);
                    moveButton.gameObject.SetActive(false);
                    regenButton.gameObject.SetActive(false);
                }
                else if (button == regenButton)
                {
                    if (selectedFurniture?.furnModel != null)
                    {
                        try
                        {
                            FurnitureData selectedFurnData = furnitureData[0];
                            // find random piece of furniture in data that matches type and instantiate it
                            if (selectedFurniture.furnData.FUR_TYPE == "Sofa") // FUR_TYPE: "Sofa"
                            {
                                selectedFurnData = sofas[Random.Range(0, sofas.Length)];
                            }
                            else if (selectedFurniture.furnData.FUR_TYPE == "Chair") // FUR_TYPE: "Chair"
                            {
                                selectedFurnData = chairs[Random.Range(0, chairs.Length)];
                            }
                            else if (selectedFurniture.furnData.FUR_TYPE == "Lamp") // FUR_TYPE: "Lamp"
                            {
                                selectedFurnData = lamps[Random.Range(0, lamps.Length)];
                            }
                            else if (selectedFurniture.furnData.FUR_TYPE == "Table") // FUR_TYPE: "Table"
                            {
                                selectedFurnData = tables[Random.Range(0, tables.Length)];
                            }
                            else if (selectedFurniture.furnData.FUR_TYPE == "Desk") // FUR_TYPE: "Desk"
                            {
                                selectedFurnData = desks[Random.Range(0, desks.Length)];
                            }
                            else if (selectedFurniture.furnData.FUR_TYPE == "Drawer") // FUR_TYPE: "Drawer"
                            {
                                selectedFurnData = drawers[Random.Range(0, drawers.Length)];
                            }
                            // instantiate the new prefab in the place of the old one, destroy old one
                            FurnitureObject newFurnitureObject = new FurnitureObject();
                            newFurnitureObject.furnData = selectedFurnData;
                            Vector3 position = selectedFurniture.furnModel.transform.position;
                            Quaternion rotation = selectedFurniture.furnModel.transform.rotation;
                            instantiatedFurniture.Remove(selectedFurniture);
                            Destroy(selectedFurniture.furnModel);
                            GameObject newPrefab = furniturePrefabs[selectedFurnData.FUR_ID - 1];
                            if (newPrefab != null)
                            {
                                newFurnitureObject.furnModel = Instantiate(newPrefab, position, rotation);
                                CollisionHandler collisionHandler = newFurnitureObject.furnModel.AddComponent<CollisionHandler>();
                                instantiatedFurniture.Add(newFurnitureObject);
                            }
                            else
                            {
                                debugText.text = "Prefab not yet created for id " + selectedFurnData.FUR_ID.ToString();
                            }

                            Renderer pRenderer = newFurnitureObject.furnModel.GetComponent<Renderer>();
                            if (pRenderer != null)
                            {
                                Bounds bounds = pRenderer.bounds;

                                // scale the object to correct dimensions
                                float scaleX = selectedFurnData.FUR_DIM_L / 100f / bounds.size.x;
                                float scaleY = selectedFurnData.FUR_DIM_W / 100f / bounds.size.y;
                                float scaleZ = selectedFurnData.FUR_DIM_H / 100f / bounds.size.z;

                                newFurnitureObject.furnModel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                            }
                            else
                            {
                                // renderer is not in parent, use the childrens renderers
                                Renderer[] childRenderers = newFurnitureObject.furnModel.GetComponentsInChildren<Renderer>();

                                if (childRenderers.Length > 0)
                                {
                                    // encapsulate the object bounds
                                    Bounds combinedBounds = childRenderers[0].bounds;
                                    for (int i = 1; i < childRenderers.Length; i++)
                                    {
                                        combinedBounds.Encapsulate(childRenderers[i].bounds);
                                    }
                                    // scale the object to correct dimensions
                                    float scaleX = selectedFurnData.FUR_DIM_L / 100f / combinedBounds.size.x;
                                    float scaleY = selectedFurnData.FUR_DIM_W / 100f / combinedBounds.size.y;
                                    float scaleZ = selectedFurnData.FUR_DIM_H / 100f / combinedBounds.size.z;

                                    newFurnitureObject.furnModel.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                                }
                            }
                            selectedFurniture = newFurnitureObject;
                            // Calculate the total cost of all instantiated furniture
                            totalCost = instantiatedFurniture.Sum(furniture => furniture.furnData.FUR_COST);

                            // Update the UI Dropdown with the total cost and individual furniture items
                            UpdateDropdown();
                        }
                        catch (Exception e)
                        {
                            debugText.text = e.Message;
                            if (errorAudio != null)
                            {
                                errorAudio.Play();
                            }
                        }
                    }
                }
                else if (button == viewModelButton)
                {
                    if (selectedFurniture?.furnData != null)
                    {
                        OpenInfoPanel();
                    }
                    else
                    {
                        if (errorAudio != null)
                        {
                            errorAudio.Play();
                        }
                    }
                }
                return;
            }
        }
        // selecting a furniture object
        if (selectedFurniture?.furnModel == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                // selected item set to first matching model in object array or null default
                selectedFurniture = instantiatedFurniture.FirstOrDefault(furnitureObject => furnitureObject.furnModel == hitObject);
                if (selectedFurniture != null)
                {
                    if (selectFurnitureAudioSource != null)
                    {
                        selectFurnitureAudioSource.Play();
                    }

                    deleteButton.gameObject.SetActive(true);
                    viewModelButton.gameObject.SetActive(true);
                    moveButton.gameObject.SetActive(true);
                    rotateButton.gameObject.SetActive(true);
                    regenButton.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            DeselectObject();
            return;
        }
    }

    private void FingerMove(EnhancedTouch.Finger finger)
    {
        if (!isDragging && !isMoveMode && !isRotateMode) return;

        // dragging a default ui sprite
        if (selectedSprite != null && !isMoveMode && !isRotateMode)
        {
            selectedSprite.transform.position = finger.screenPosition;
        }

        // dragging a furniture object
        if (isMoveMode)
        {
            if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hits)
                {
                    if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        selectedFurniture.furnModel.transform.position = hit.pose.position;
                    }
                }
            }
        }

        // rotating a furniture object
        if (isRotateMode)
        {
            Vector2 delta = finger.screenPosition - initialFingerPosition;
            float rotationAngle = delta.x * -0.5f;

            selectedFurniture.furnModel.transform.rotation = objRotationBefore * Quaternion.Euler(0, rotationAngle, 0);
        }
    }

    private void FingerUp(EnhancedTouch.Finger finger)
    {
        if (!isDragging && !isMoveMode && !isRotateMode) return;

        isDragging = false;

        // placing the default sprite and converting to a furniture object
        if (selectedSprite != null && !isMoveMode)
        {
            selectedSprite.gameObject.SetActive(false);
            int selectedIndex = Array.IndexOf(uiSprites, selectedSprite);
            // ui sprite was dropped on an existing ar plane
            if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hits)
                {
                    if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        Pose pose = hit.pose;
                        try
                        {
                            FurnitureData selectedFurn = furnitureData[0];
                            // find random piece of furniture in data that matches type and instantiate it
                            if (selectedIndex == 0) // FUR_TYPE: "Sofa"
                            {
                                selectedFurn = sofas[Random.Range(0, sofas.Length)];
                            }
                            else if (selectedIndex == 1) // FUR_TYPE: "Chair"
                            {
                                selectedFurn = chairs[Random.Range(0, chairs.Length)];
                            }
                            else if (selectedIndex == 2) // FUR_TYPE: "Lamp"
                            {
                                selectedFurn = lamps[Random.Range(0, lamps.Length)];
                            }
                            else if (selectedIndex == 3) // FUR_TYPE: "Table"
                            {
                                selectedFurn = tables[Random.Range(0, tables.Length)];
                            }
                            else if (selectedIndex == 4) // FUR_TYPE: "Desk"
                            {
                                selectedFurn = desks[Random.Range(0, desks.Length)];
                            }
                            else if (selectedIndex == 5) // FUR_TYPE: "Drawer"
                            {
                                selectedFurn = drawers[Random.Range(0, drawers.Length)];
                            }
                            FurnitureObject newFurnitureObject = new FurnitureObject();
                            newFurnitureObject.furnData = selectedFurn;
                            GameObject newPrefab = furniturePrefabs[selectedFurn.FUR_ID - 1];
                            if (newPrefab != null)
                            {
                                newFurnitureObject.furnModel = Instantiate(newPrefab, pose.position, hit.pose.rotation * Quaternion.Euler(Vector3.up * 180));
                                CollisionHandler collisionHandler = newFurnitureObject.furnModel.AddComponent<CollisionHandler>();
                                instantiatedFurniture.Add(newFurnitureObject);
                            }
                            else
                            {
                                debugText.text = "Prefab not yet created for id " + selectedFurn.FUR_ID.ToString();
                            }

                            // scale the object to correct dimensions
                            // get object's renderer to measure its current size
                            // check if renderer is on the parent object first
                            Renderer pRenderer = newFurnitureObject.furnModel.GetComponent<Renderer>();
                            if (pRenderer != null)
                            {
                                Bounds bounds = pRenderer.bounds;

                                // get the scale factors for each dimension
                                float scaleX = selectedFurn.FUR_DIM_L / 100f / bounds.size.x;
                                float scaleY = selectedFurn.FUR_DIM_W / 100f / bounds.size.y;
                                float scaleZ = selectedFurn.FUR_DIM_H / 100f / bounds.size.z;

                                // average the scale factors to get one factor
                                float avgScale = (scaleX + scaleY + scaleZ) / 3;

                                // apply scale to object
                                newFurnitureObject.furnModel.transform.localScale = new Vector3(avgScale, avgScale, avgScale);
                            }
                            else
                            {
                                // renderer is not in parent, use the children's renderers
                                Renderer[] childRenderers = newFurnitureObject.furnModel.GetComponentsInChildren<Renderer>();

                                if (childRenderers.Length > 0)
                                {
                                    // encapsulate the object bounds
                                    Bounds combinedBounds = childRenderers[0].bounds;
                                    for (int i = 1; i < childRenderers.Length; i++)
                                    {
                                        combinedBounds.Encapsulate(childRenderers[i].bounds);
                                    }

                                    // get the scale factors for each dimension
                                    float scaleX = selectedFurn.FUR_DIM_L / 100f / combinedBounds.size.x;
                                    float scaleY = selectedFurn.FUR_DIM_W / 100f / combinedBounds.size.y;
                                    float scaleZ = selectedFurn.FUR_DIM_H / 100f / combinedBounds.size.z;

                                    // average the scale factors to get one factor
                                    float avgScale = (scaleX + scaleY + scaleZ) / 3;

                                    // apply scale to object
                                    newFurnitureObject.furnModel.transform.localScale = new Vector3(avgScale, avgScale, avgScale);
                                }
                            }
                            // Calculate the total cost of all instantiated furniture
                            totalCost = instantiatedFurniture.Sum(furniture => furniture.furnData.FUR_COST);

                            // Update the UI Dropdown with the total cost and individual furniture items
                            UpdateDropdown();
                        }
                        catch (Exception e)
                        {
                            debugText.text = e.Message;
                            if (errorAudio != null)
                            {
                                errorAudio.Play();
                            }
                        }

                        if (dropFurnitureAudioSource != null)
                        {
                            dropFurnitureAudioSource.Play();
                        }
                    }
                }
            }
            else
            {
                if (errorAudio != null)
                {
                    errorAudio.Play();
                }
            }
            selectedSprite.rectTransform.localPosition = Vector3.zero;
            selectedSprite = null;
        }
        // object placed after being dragging in move mode
        else if (isMoveMode)
        {
            isMoveMode = false;
            // check that is was dropped on a valid ar plane
            if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hits)
                {
                    if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        selectedFurniture.furnModel.transform.position = hit.pose.position;
                        if (dropFurnitureAudioSource != null)
                        {
                            dropFurnitureAudioSource.Play();
                        }
                    }
                }
            }
            else
            {
                // reset pose if not on a valid plane
                selectedFurniture.furnModel.transform.position = objPositionBeforeMove;
                if (errorAudio != null)
                {
                    errorAudio.Play();
                }
            }
            deleteButton.gameObject.SetActive(true);
            viewModelButton.gameObject.SetActive(true);
            rotateButton.gameObject.SetActive(true);
            regenButton.gameObject.SetActive(true);
        }
        // figner released after ar mode, position already set during drag and will be valid due to 
        // no plane change
        else if (isRotateMode)
        {
            isRotateMode = false;
            deleteButton.gameObject.SetActive(true);
            viewModelButton.gameObject.SetActive(true);
            moveButton.gameObject.SetActive(true);
            regenButton.gameObject.SetActive(true);
            if (dropFurnitureAudioSource != null)
            {
                dropFurnitureAudioSource.Play();
            }
        }
    }


    private void DeselectObject()
    {
        deleteButton.gameObject.SetActive(false);
        viewModelButton.gameObject.SetActive(false);
        moveButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        regenButton.gameObject.SetActive(false);

        selectedFurniture = null;
    }

    private void DeleteObject()
    {
        if (selectedFurniture?.furnModel != null)
        {
            totalCost -= selectedFurniture.furnData.FUR_COST;
            // move the object out of the scene
            selectedFurniture.furnModel.transform.position = new Vector3(-10000, -10000, -10000);
            StartCoroutine(DestroyObjectAfterFixedUpdate());
        }
    }

    private IEnumerator DestroyObjectAfterFixedUpdate()
    {
        // wait for the next FixedUpdate to register collision exit
        yield return new WaitForFixedUpdate();
        instantiatedFurniture.Remove(selectedFurniture);
        Destroy(selectedFurniture.furnModel);

        // Update the UI Dropdown with the new total cost
        UpdateDropdown();

        DeselectObject();
    }

    public void CloseUpdatePanel()
    {
        if (uiButtonAudioSource != null)
        {
            uiButtonAudioSource.Play();
        }
        updateDisplay.SetActive(false);
        updateDisplay.transform.localPosition = new Vector3(9999f, 0f, 0f);
    }

    private void OpenInfoPanel()
    {
        showingInfoPanel = true;
        infoImage.sprite = furnitureSprites[selectedFurniture.furnData.FUR_ID - 1];
        infoName.text = selectedFurniture.furnData.FUR_NAME;
        infoDesc.text = selectedFurniture.furnData.FUR_DESC;
        infoCost.text = "$" + selectedFurniture.furnData.FUR_COST.ToString();
        infoDims.text = selectedFurniture.furnData.FUR_DIM_L.ToString() + "x" + selectedFurniture.furnData.FUR_DIM_W.ToString() + "x" + selectedFurniture.furnData.FUR_DIM_H.ToString() + " cm";
        productUrl = selectedFurniture.furnData.FUR_LINK.Replace("\\/", "/").Replace("\n", "").Replace("\r", "");
        if (favFurnIds.Contains(selectedFurniture.furnData.FUR_ID))
            favoriteButtonImage.sprite = filledHeart;
        else
            favoriteButtonImage.sprite = unfilledHeart;
        infoPanel.transform.localPosition = Vector3.zero;
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        showingInfoPanel = false;
        infoPanel.SetActive(false);
        infoPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);
    }

    public void OpenProductLink()
    {
        Application.OpenURL(productUrl);
    }

    public void ClickedFavoriteButton()
    {
        int selectedFurnitureId = selectedFurniture.furnData.FUR_ID;
        // check if in array
        if (favFurnIds.Contains(selectedFurnitureId))
        {
            // if in, remove and change sprite to empty
            favFurnIds = favFurnIds.Where(id => id != selectedFurnitureId).ToArray();
            favoriteButtonImage.sprite = unfilledHeart;
        }
        else
        {
            // if not in, add and cahnge sprite to filled
            favFurnIds = favFurnIds.Concat(new[] { selectedFurnitureId }).ToArray();
            favoriteButtonImage.sprite = filledHeart;
        }

        // save array back to the file
        SaveFavoriteFurnitureIds();
    }

    private void LoadFavoriteFurnitureIds()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "favFurnIds.txt");

        if (File.Exists(filePath))
        {
            string[] idStrings = File.ReadAllLines(filePath);
            favFurnIds = new int[idStrings.Length];

            for (int i = 0; i < idStrings.Length; i++)
            {
                if (int.TryParse(idStrings[i], out int id))
                {
                    favFurnIds[i] = id;
                }
                else
                {
                    Debug.LogError("Error parsing furniture ID from file.");
                }
            }
        }
        else
        {
            favFurnIds = new int[0];
        }
    }

    private void SaveFavoriteFurnitureIds()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "favFurnIds.txt");
        File.WriteAllLines(filePath, favFurnIds.Select(id => id.ToString()).ToArray());
    }

    private void OnDestroy()
    {
        StopAllCoroutines(); // Stop the coroutine when the script is destroyed
    }
}
