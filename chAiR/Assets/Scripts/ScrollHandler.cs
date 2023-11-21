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

public class ScrollViewManager : MonoBehaviour
{
    public GameObject prefab; // Prefab of the object you want to add to the scroll view
    public Transform content; // Content transform of the scroll view
    // public int numberOfObjects = 10; // Number of objects you want to add
    // public GameObject[] furnitureItems;
    //public Sprite[] furnImages; 
    private List<FurnitureData> furnitureData;
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

    void Start()
    {
        // Call a method to populate the scroll view with objects
        StartCoroutine(PopulateScrollView());
    }

    IEnumerator PopulateScrollView()
    {
        // Loop through the number of objects you want to add
        Debug.Log("PopulateScrollView started");
        int retryCount = 0;
        string apiUrl = "https://hammy-exchanges.000webhostapp.com/all_furn.php";
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    /*
                    loadingPanel.SetActive(true);
                    */
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
                        Debug.Log(furnitureData.Count);
                        for(int i = 0; i < furnitureData.Count; i++)
                        {
                            GameObject newObject = Instantiate(prefab, content);
                            Text[] textComponent = newObject.GetComponentsInChildren<Text>();
                            if (textComponent != null)
                            {
                                textComponent[0].text = furnitureData[i].FUR_NAME;
                                textComponent[1].text = furnitureData[i].FUR_ID.ToString(); 
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
        /*
        for(int i = 0; i < furnitureItems.Length; i++)
        {
            GameObject furniture = furnitureItems[i];
            GameObject newObject = Instantiate(prefab, content);
            Text textComponent = newObject.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = furniture.name;
            }

            Image childImage = newObject.GetComponentsInChildren<Image>()[1];

            // Check if the child Image component is found
            if (childImage != null)
            {
                // Set the sprite of the child Image component to the new sprite
                childImage.sprite = furnImages[i];
                Debug.Log("Image Identified");

                // Optionally, you can do other things with the childImage, such as changing its color or other properties.
            }
            else
            {
                Debug.LogError("Child Image component not found in the prefab.");
            }
            
        }
        
        */
        // Calculate the size of the content based on the number of objects and their size
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float contentHeight = furnitureData.Count * prefab.GetComponent<RectTransform>().rect.height;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }
}
