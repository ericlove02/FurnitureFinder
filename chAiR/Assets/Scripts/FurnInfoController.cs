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

public class FurnInfoController : MonoBehaviour
{
    public Image oldImage;
    public Image newImage;
    public Text furnitureName;
    public Text furnitureCostText;
    public static string furniturePurchaseLink;
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
        StartCoroutine(UpdatePage());
    }

    IEnumerator UpdatePage()
    {
        // Loop through the number of objects you want to add
        int id = SelectedFurniture.clickedFurnitureId; 
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
                        FurnitureData furniture = furnitureData[id - 1];
                        furnitureName.text = furniture.FUR_NAME;
                        furnitureCostText.text = "$" + furniture.FUR_COST.ToString();
                        furniturePurchaseLink = furniture.FUR_LINK;
                    }
                    catch (Exception e)
                    {
                        debugText.text = e.Message;
                    }
                }
            }
    }

    public void OpenWebpageOnClick()
    {
        Application.OpenURL(FurnInfoController.furniturePurchaseLink);
    }
}