using System.Collections.Generic;
using System.Collections;
using UnityEngine;
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

    // array to hold available prefab options
    // we will store all of the indices of the prefabs to the db and use that to retrieve the correct
    // prefab for the piece of furniture
    public GameObject[] furniturePrefabs;

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
    private FurnitureData[] tvStands;


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

        // retrive stored vibe or if not set, vibeError
        selectedVibe = PlayerPrefs.GetString("Vibe", "VibeERROR");
        StartCoroutine(GetFurnitureData(selectedVibe));
    }

    private IEnumerator GetFurnitureData(string vibeName)
    {
        string apiUrl = "https://hammy-exchanges.000webhostapp.com/index.php?vibe_name=" + vibeName;

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // parse JSON and store data
                string json = www.downloadHandler.text;
                try
                {
                    furnitureData = JsonConvert.DeserializeObject<List<FurnitureData>>(json);
                    sofas = furnitureData.Where(furniture => furniture.FUR_TYPE == "Sofa").ToArray();
                    chairs = furnitureData.Where(furniture => furniture.FUR_TYPE == "Chair").ToArray();
                    lamps = furnitureData.Where(furniture => furniture.FUR_TYPE == "Lamp").ToArray();
                    tables = furnitureData.Where(furniture => furniture.FUR_TYPE == "Table").ToArray();
                    tvStands = furnitureData.Where(furniture => furniture.FUR_TYPE == "TV Stand").ToArray();
                }
                catch (Exception e)
                {
                    debugText.text = e.Message;
                }

            }
        }
    }

    private void Update()
    {
        // if a furn obj is select, move ui elements to follow on each frame update
        if (selectedFurniture?.furnModel != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedFurniture.furnModel.transform.position);

            deleteButton.transform.position = screenPos + new Vector3(0, 200, 0);
            viewModelButton.transform.position = screenPos + new Vector3(200, 200, 0);
            moveButton.transform.position = screenPos + new Vector3(0, 0, 0);
            rotateButton.transform.position = screenPos + new Vector3(200, 0, 0);
            regenButton.transform.position = screenPos + new Vector3(100, 0, 0);
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
        if (isDragging) return;

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
                            else if (selectedFurniture.furnData.FUR_TYPE == "TV Stand") // FUR_TYPE: "TV Stand"
                            {
                                selectedFurnData = tvStands[Random.Range(0, tvStands.Length)];
                            }
                            // instantiate the new prefab in the place of the old one, destroy old one
                            FurnitureObject newFurnitureObject = new FurnitureObject();
                            newFurnitureObject.furnData = selectedFurnData;
                            Vector3 position = selectedFurniture.furnModel.transform.position;
                            Quaternion rotation = selectedFurniture.furnModel.transform.rotation;
                            instantiatedFurniture.Remove(selectedFurniture);
                            Destroy(selectedFurniture.furnModel);
                            newFurnitureObject.furnModel = Instantiate(furniturePrefabs[selectedFurnData.FUR_ID - 1], position, rotation);
                            CollisionHandler collisionHandler = newFurnitureObject.furnModel.AddComponent<CollisionHandler>();
                            instantiatedFurniture.Add(newFurnitureObject);
                            selectedFurniture = newFurnitureObject;

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
                            else if (selectedIndex == 4) // FUR_TYPE: "TV Stand"
                            {
                                selectedFurn = tvStands[Random.Range(0, tvStands.Length)];
                            }
                            FurnitureObject newFurnitureObject = new FurnitureObject();
                            newFurnitureObject.furnData = selectedFurn;
                            newFurnitureObject.furnModel = Instantiate(furniturePrefabs[selectedFurn.FUR_ID - 1], pose.position, hit.pose.rotation * Quaternion.Euler(Vector3.up * 180));
                            CollisionHandler collisionHandler = newFurnitureObject.furnModel.AddComponent<CollisionHandler>();
                            instantiatedFurniture.Add(newFurnitureObject);
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
        DeselectObject();
    }
}
