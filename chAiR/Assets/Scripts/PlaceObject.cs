using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;
// using TMPro;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject[] objPrefabs;
    [SerializeField] private Image[] uiSprites;

    [SerializeField] private Button deleteButton;
    [SerializeField] private Button viewModelButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button rotateButton;

    [SerializeField] private AudioSource furnitureAudioSource;
    [SerializeField] private AudioSource uiButtonAudioSource;

    private bool isDragging = false;
    private Image selectedSprite;
    private GameObject selectedObject;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<GameObject> instantiatedModels = new List<GameObject>();
    // [SerializeField] TMP_Text debugText;

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
    }

    private void Update()
    {
        if (selectedObject != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedObject.transform.position);

            deleteButton.transform.position = screenPos + new Vector3(0, 200, 0);
            viewModelButton.transform.position = screenPos + new Vector3(200, 200, 0);
            moveButton.transform.position = screenPos + new Vector3(0, 0, 0);
            rotateButton.transform.position = screenPos + new Vector3(200, 0, 0);
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

        foreach (Button button in new Button[] { deleteButton, viewModelButton, moveButton, rotateButton })
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), finger.screenPosition))
            {
                if (button == deleteButton)
                {
                    DeleteObject();
                }
                return;
            }
        }

        if (selectedObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (instantiatedModels.Contains(hitObject))
                {
                    if (furnitureAudioSource != null)
                    {
                        furnitureAudioSource.Play();
                    }
                    selectedObject = hitObject;

                    deleteButton.gameObject.SetActive(true);
                    viewModelButton.gameObject.SetActive(true);
                    moveButton.gameObject.SetActive(true);
                    rotateButton.gameObject.SetActive(true);
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
        if (!isDragging) return;

        if (selectedSprite != null)
        {
            selectedSprite.transform.position = finger.screenPosition;
        }
    }

    private void FingerUp(EnhancedTouch.Finger finger)
    {
        if (!isDragging) return;

        isDragging = false;

        if (selectedSprite != null)
        {
            selectedSprite.gameObject.SetActive(false);
            int selectedIndex = Array.IndexOf(uiSprites, selectedSprite);

            if (selectedIndex >= 0 && selectedIndex < objPrefabs.Length)
            {
                if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
                {
                    foreach (ARRaycastHit hit in hits)
                    {
                        if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                        {
                            Pose pose = hit.pose;
                            GameObject obj = Instantiate(objPrefabs[selectedIndex], pose.position, hit.pose.rotation * Quaternion.Euler(Vector3.up * 180));
                            instantiatedModels.Add(obj);
                        }
                    }
                }
            }
            selectedSprite.rectTransform.localPosition = Vector3.zero;
            selectedSprite = null;
        }
    }

    private void DeselectObject()
    {
        deleteButton.gameObject.SetActive(false);
        viewModelButton.gameObject.SetActive(false);
        moveButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);

        selectedObject = null;
    }

    private void DeleteObject()
    {
        if (selectedObject != null)
        {
            if (uiButtonAudioSource != null)
            {
                uiButtonAudioSource.Play();
            }
            Destroy(selectedObject);
            DeselectObject();
        }
    }
}
