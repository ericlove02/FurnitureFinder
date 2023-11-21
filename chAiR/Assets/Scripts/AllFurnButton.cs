using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;

public class ButtonClickScript : MonoBehaviour
{
    // Set the scene name you want to load in the inspector
    public string furniturePurchaseLink;
    Canvas canvas; 
    GameObject panel;

    void Start()
    {
        // Attach the method to the button's onClick event
        canvas = FindCanvasInHierarchy(transform);
        Transform panelTransform = canvas.transform.Find("Panel");
        panel = panelTransform.gameObject;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Method to be called when the button is clicked
    void OnClick()
    {
        // Find the Text component among the children
        Transform parentTransform = transform.parent;

        Text[] textComponent = parentTransform.GetComponentsInChildren<Text>();

        // Check if the Text component is found
        if (textComponent != null)
        {
            // Get the text value from the Text component
            string name = textComponent[0].text;
            string id = textComponent[1].text;

            // Set the text value to the static variable and record the page that we came from
            SelectedFurniture.clickedFurnitureName = name;
            int furnitureId;
            int.TryParse(id, out furnitureId);
            SelectedFurniture.clickedFurnitureId = furnitureId;
            SelectedFurniture.previousPage = "AllFurn";

            // Print the static variable for verification
            Debug.Log("Name: " + SelectedFurniture.clickedFurnitureName);
            Debug.Log("ID: " + SelectedFurniture.clickedFurnitureId);

            // Change the scene
            List<ScrollViewManager.FurnitureData> furnitureData = ScrollViewManager.furnitureData;
            ScrollViewManager.FurnitureData selectedFurniture = furnitureData[furnitureId - 1];

            Text furnitureName = panel.transform.Find("Furniture Name").GetComponent<Text>(); // Replace "TextObjectName" with the actual name of your Text component
            Text furnitureCost = panel.transform.Find("Furniture Cost").GetComponent<Text>(); // Replace "TextObjectName" with the actual name of your Text component
            furnitureName.text = selectedFurniture.FUR_NAME;
            furnitureCost.text = selectedFurniture.FUR_COST.ToString();
            furniturePurchaseLink = selectedFurniture.FUR_LINK.Replace(@"\/", "/").Replace("\n", "").Replace("\r", "");
            panel.SetActive(!panel.activeSelf);
            

        }
        else
        {
            Debug.LogError("Text component not found among the children.");
        }
    }

    Canvas FindCanvasInHierarchy(Transform obj)
    {
        if (obj == null)
        {
            return null;
        }

        // Check if the current object has a canvas component
        Canvas canvas = obj.GetComponent<Canvas>();

        if (canvas != null)
        {
            // Canvas found, return it
            return canvas;
        }
        else
        {
            // If the current object doesn't have the canvas, recursively check its parent
            return FindCanvasInHierarchy(obj.parent);
        }
    }

    public void OpenWebpageOnClick()
    {
        Application.OpenURL(furniturePurchaseLink);
        Debug.Log(furniturePurchaseLink);
    }
}
