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

public class FavoriteHandler : MonoBehaviour
{
    public GameObject prefab; // Prefab of the object you want to add to the scroll view
    public Transform content; // Content transform of the scroll view
    public Sprite[] furnitureSprites;
    public static List<FurnitureData> furnitureData;
    public Sprite unfilledHeart;
    public Sprite filledHeart;
    public Button likeButton;
    public GameObject panelInScene;
    public static GameObject staticPanel;
    [SerializeField] TMP_Text debugText;
    private int[] favFurnIds;

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
        LoadFavoriteFurnitureIds();
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
                        int tempFurnitureId = furnitureData[i].FUR_ID;
                        if(favFurnIds.Contains(tempFurnitureId))
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

    public void FavoriteFurniture()
    {
        int selectedFurnitureId = SelectedFurniture.clickedFurnitureId;
        // check if in array
        if (favFurnIds.Contains(selectedFurnitureId))
        {
            // if in, remove and change sprite to empty
            favFurnIds = favFurnIds.Where(id => id != selectedFurnitureId).ToArray();
            likeButton.GetComponent<Image>().sprite = unfilledHeart;
        }
        else
        {
            // if not in, add and cahnge sprite to filled
            favFurnIds = favFurnIds.Concat(new[] { selectedFurnitureId }).ToArray();
            likeButton.GetComponent<Image>().sprite = filledHeart;
        }

        // save array back to the file
        SaveFavoriteFurnitureIds();
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
