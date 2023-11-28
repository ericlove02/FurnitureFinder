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

public class ButtonClickScript : MonoBehaviour
{
    // Set the scene name you want to load in the inspector
    public string furniturePurchaseLink;
    Canvas canvas;
    GameObject panel;
    Sprite unfilledFavButton;
    Sprite fillFavButton;
    private int[] favFurnIds;

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
        LoadFavoriteFurnitureIds();

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

            TMP_Text furnitureName = panel.transform.Find("Furniture Name").GetComponent<TMP_Text>(); // Replace "TextObjectName" with the actual name of your Text component
            TMP_Text furnitureCost = panel.transform.Find("Furniture Cost").GetComponent<TMP_Text>(); // Replace "TextObjectName" with the actual name of your Text component
            Image furnitureSprite = panel.transform.Find("FurnImage").GetComponent<Image>(); // Replace "TextObjectName" with the actual name of your Text component
            Button favoriteButton = panel.transform.Find("FavoriteButton").GetComponent<Button>();
            unfilledFavButton = panel.transform.Find("UnfilledFavoriteButton").GetComponent<Button>().GetComponent<Image>().sprite;
            fillFavButton = panel.transform.Find("FilledFavoriteButton").GetComponent<Button>().GetComponent<Image>().sprite; // Replace "TextObjectName" with the actual name of your Text component
            TMP_Text furnitureDims = panel.transform.Find("Dimensions").GetComponent<TMP_Text>();
            TMP_Text furnitureDesc = panel.transform.Find("Desc").GetComponent<TMP_Text>();

            if (furnitureId < SelectedFurniture.furniturePics.Length)
            {
                furnitureSprite.sprite = SelectedFurniture.furniturePics[furnitureId - 1];
            }

            if (favFurnIds.Contains(furnitureId))
            {
                favoriteButton.GetComponent<Image>().sprite = fillFavButton;
            }else
            {
                favoriteButton.GetComponent<Image>().sprite = unfilledFavButton;
            }

            furnitureName.text = selectedFurniture.FUR_NAME;
            furnitureCost.text = "$" + selectedFurniture.FUR_COST.ToString();
            SelectedFurniture.furniturePurchaseLink = selectedFurniture.FUR_LINK.Replace(@"\/", "/").Replace("\n", "").Replace("\r", "");
            furnitureDims.text = selectedFurniture.FUR_DIM_L.ToString() + "x" + selectedFurniture.FUR_DIM_W.ToString() + "x" + selectedFurniture.FUR_DIM_H.ToString() + " cm";
            furnitureDesc.text = selectedFurniture.FUR_DESC;
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

        Canvas canvas = obj.GetComponent<Canvas>();

        if (canvas != null)
        {
            return canvas;
        }
        else
        {
            return FindCanvasInHierarchy(obj.parent);
        }
    }

    public void OpenWebpageOnClick()
    {
        Application.OpenURL(SelectedFurniture.furniturePurchaseLink);
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
}
