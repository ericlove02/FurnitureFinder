using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;
using TMPro;
using System.IO;
using Random = UnityEngine.Random;
using Dummiesman; // OBJLoader, MTLLoader
using System.Text;

public class ObjectHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] objPrefabs;
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
    private GameObject selectedObject;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<GameObject> instantiatedModels = new List<GameObject>();
    private Vector3 objPositionBeforeMove;
    // for rotation
    private Quaternion objRotationBefore;
    private Vector2 initialFingerPosition;

    private string selectedVibe;

    // array to hold available prefab options
    // we will store all of the indices of the prefabs to the db and use that to retrieve the correct
    // prefab for the piece of furniture
    public GameObject[] furniturePrefabs;
    private int selectedPrefabIndex = -1;

    [SerializeField] TMP_Text debugText;

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
    }

    private void Update()
    {
        // if a furn obj is select, move ui elements to follow on each frame update
        if (selectedObject != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedObject.transform.position);

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
                    objPositionBeforeMove = selectedObject.transform.position;
                    // hide buttons in move mode
                    deleteButton.gameObject.SetActive(false);
                    viewModelButton.gameObject.SetActive(false);
                    rotateButton.gameObject.SetActive(false);
                    regenButton.gameObject.SetActive(false);
                }
                else if (button == rotateButton)
                {
                    isRotateMode = true;
                    objRotationBefore = selectedObject.transform.rotation;
                    initialFingerPosition = finger.screenPosition;
                    // hide buttons in rotate mode
                    deleteButton.gameObject.SetActive(false);
                    viewModelButton.gameObject.SetActive(false);
                    moveButton.gameObject.SetActive(false);
                    regenButton.gameObject.SetActive(false);
                }
                else if (button == regenButton)
                {
                    if (selectedObject != null)
                    {
                        int randomIndex;
                        do
                        {
                            // get a model different from current one
                            randomIndex = Random.Range(0, furniturePrefabs.Length);
                        } while (randomIndex == selectedPrefabIndex);
                        selectedPrefabIndex = randomIndex;

                        // instantiate the new prefab in the place of the old one, destroy old one
                        GameObject newPrefab = furniturePrefabs[randomIndex];
                        Vector3 position = selectedObject.transform.position;
                        Quaternion rotation = selectedObject.transform.rotation;
                        Destroy(selectedObject);
                        selectedObject = Instantiate(newPrefab, position, rotation);
                        instantiatedModels.Add(selectedObject);
                    }
                }
                return;
            }
        }
        // selecting a furniture object
        if (selectedObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (instantiatedModels.Contains(hitObject))
                {
                    if (selectFurnitureAudioSource != null)
                    {
                        selectFurnitureAudioSource.Play();
                    }
                    selectedObject = hitObject;

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
                        selectedObject.transform.position = hit.pose.position;
                    }
                }
            }
        }

        // rotating a furniture object
        if (isRotateMode)
        {
            Vector2 delta = finger.screenPosition - initialFingerPosition;
            float rotationAngle = delta.x * -0.5f;

            selectedObject.transform.rotation = objRotationBefore * Quaternion.Euler(0, rotationAngle, 0);
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

            if (selectedIndex >= 0 && selectedIndex < objPrefabs.Length)
            {
                // ui sprite was dropped on an existing ar plane
                if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
                {
                    foreach (ARRaycastHit hit in hits)
                    {
                        if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                        {
                            Pose pose = hit.pose;

                            // temp for db and regen test
                            // if placed object is sofa dont place a filler, place real object
                            GameObject obj;
                            if (selectedIndex == 0)
                            {
                                GameObject randomPrefab = furniturePrefabs[Random.Range(0, furniturePrefabs.Length)];
                                obj = Instantiate(randomPrefab, pose.position, hit.pose.rotation * Quaternion.Euler(Vector3.up * 180));
                            }
                            else if (selectedIndex == 1)
                            {
                                // load obj
                                var objLoad = new WWW("https://people.tamu.edu/~eric.love02/test_obj/IKEA-Ektorp_Armchair_Vallsta_Red-3D.obj");
                                var mtlLoad = new WWW("https://people.tamu.edu/~eric.love02/test_obj/IKEA-Ektorp_Armchair_Vallsta_Red-3D.mtl");
                                while (!objLoad.isDone || !mtlLoad.isDone)
                                    System.Threading.Thread.Sleep(1);

                                // create stream and load
                                var objStream = new MemoryStream(Encoding.UTF8.GetBytes(objLoad.text));
                                var mtlStream = new MemoryStream(Encoding.UTF8.GetBytes(mtlLoad.text));
                                obj = new OBJLoader().Load(objStream, mtlStream);

                                obj.transform.position = pose.position;
                                obj.transform.rotation = hit.pose.rotation * Quaternion.Euler(Vector3.up * 180);

                                // add box collider
                                BoxCollider boxCollider = obj.AddComponent<BoxCollider>();

                                if (obj)
                                {
                                    Bounds bounds = CalculateBoundingBox(obj);
                                    boxCollider.size = bounds.size;

                                    float maxSize = 1.0f;
                                    float scaleFactor = maxSize / Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
                                    obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

                                    debugText.text = obj.name + "\n" + obj.transform.position + "\n" + bounds.size.x + ", " + bounds.size.y;
                                }
                                else
                                {
                                    debugText.text = "object null";
                                }
                            }
                            else
                            {
                                obj = Instantiate(objPrefabs[selectedIndex], pose.position, hit.pose.rotation * Quaternion.Euler(Vector3.up * 180));
                            }
                            instantiatedModels.Add(obj);

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
                        selectedObject.transform.position = hit.pose.position;
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
                selectedObject.transform.position = objPositionBeforeMove;
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

        selectedObject = null;
    }

    private void DeleteObject()
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
            DeselectObject();
        }
    }

    private Bounds CalculateBoundingBox(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            return bounds;
        }
        return new Bounds(obj.transform.position, Vector3.zero);
    }
}
