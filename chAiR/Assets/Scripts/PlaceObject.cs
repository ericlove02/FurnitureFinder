using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject[] objPrefabs;
    [SerializeField] private Image[] uiSprites;

    private bool isDragging = false;
    private Image selectedSprite;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        foreach (Image sprite in uiSprites)
        {
            sprite.gameObject.SetActive(false);

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
                            GameObject obj = Instantiate(objPrefabs[selectedIndex], pose.position, pose.rotation);
                        }
                    }
                }
            }

            selectedSprite = null;
        }
    }
}
