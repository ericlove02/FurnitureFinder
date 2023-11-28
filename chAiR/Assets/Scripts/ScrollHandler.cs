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
    public Sprite[] furnitureSprites;
    public static List<FurnitureData> furnitureData;
    public GameObject panelInScene;
    public static GameObject staticPanel;
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
    private float scaleMultiplier = 1f;

    void Start()
    {
        panelInScene.SetActive(false);
        SelectedFurniture.furniturePics = furnitureSprites;
        StartCoroutine(PopulateScrollView());
        staticPanel = panelInScene;
    }

    IEnumerator PopulateScrollView()
    {
        int retryCount = 0;
        string apiUrl = "https://hammy-exchanges.000webhostapp.com/all_furn.php";
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                yield return new WaitForSeconds(3);
                retryCount++;
            }
            else
            {
                string json = www.downloadHandler.text;
                try
                {
                    furnitureData = JsonConvert.DeserializeObject<List<FurnitureData>>(json);
                    for (int i = 0; i < furnitureData.Count; i++)
                    {

                        GameObject newObject = Instantiate(prefab, content);
                        Text[] textComponent = newObject.GetComponentsInChildren<Text>();
                        Image[] furnitureImages = newObject.GetComponentsInChildren<Image>();
                        textComponent[0].text = furnitureData[i].FUR_NAME;
                        textComponent[1].text = furnitureData[i].FUR_ID.ToString();
                        if (i < SelectedFurniture.furniturePics.Length)
                        {
                            furnitureImages[1].sprite = SelectedFurniture.furniturePics[i];
                        }
                        RectTransform buttonRectTransform = newObject.GetComponent<RectTransform>();
                        /*
                        buttonRectTransform.anchorMin = new Vector2(0, 0.5f);
                        buttonRectTransform.anchorMax = new Vector2(1, 0.5f);
                        buttonRectTransform.sizeDelta = new Vector2(0, newObject.GetComponent<RectTransform>().sizeDelta.y);
                        */

                        // Resize the prefab to fit the scroll view
                        float targetWidth = content.GetComponent<RectTransform>().rect.width;

                        // Calculate the target scale based on the target width and the aspect ratio
                        float prefabWidth = newObject.GetComponent<RectTransform>().rect.width;
                        float prefabHeight = newObject.GetComponent<RectTransform>().rect.height;
                        float targetAspect = targetWidth / prefabHeight;
                        float currentAspect = prefabWidth / prefabHeight;
                        float scaleMultiplier = targetAspect / currentAspect;

                        // Apply the scale while maintaining the aspect ratio
                        newObject.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1f);
                        // After setting the scale, adjust the localPosition
                        float yOffset = i * (newObject.GetComponent<RectTransform>().rect.height * scaleMultiplier);
                        newObject.transform.localPosition = new Vector3(newObject.transform.localPosition.x, -yOffset, newObject.transform.localPosition.z);

                    }
                    yield break;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float totalHeight = furnitureData.Count * prefab.GetComponent<RectTransform>().rect.height * scaleMultiplier;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
    }

    public void OpenWebpageOnClick()
    {
        Application.OpenURL(SelectedFurniture.furniturePurchaseLink);
    }

    public void TogglePanel()
    {
        SelectedFurniture.furniturePurchaseLink = "";
        panelInScene.SetActive(!panelInScene.activeSelf);
    }
}
