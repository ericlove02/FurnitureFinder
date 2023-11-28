using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePanelHeight : MonoBehaviour
{
    void Update()
    {
        // Get the RectTransform component of the object
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // Set the height of the RectTransform to the full height of the device
            SetFullHeight(rectTransform);
        }
        else
        {
            Debug.LogError("RectTransform component not found on the object.");
        }
    }

    void SetFullHeight(RectTransform rectTransform)
    {
        // Get the screen height
        float screenHeight = Screen.height;

        // Set the height of the RectTransform to the screen height
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, screenHeight);
    }
}
