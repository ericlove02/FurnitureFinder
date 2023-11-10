using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFeatureHandler : MonoBehaviour
{
    public Sprite[] images;
    public GameObject imagePrefab; // Prefab for creating images

    private Sprite[] vibeImages;
    private GameObject[] imageObjects;
    private float imageWidth = 900f;
    private float imageHeight = 900f;
    private float imageSpeed = 100f;
    private int previousSelectedIndex = 0;

    void Start()
    {
        previousSelectedIndex = PlayerPrefs.GetInt("SelectedVibeIndex", 0);
        // Set vibeImages initially
        UpdateVibeImages();
        CreateImageObjects();
    }

    void UpdateVibeImages()
    {
        switch (PlayerPrefs.GetInt("SelectedVibeIndex", 0))
        {
            case 0:
                // minimalist
                vibeImages = images[0..4];
                break;
            case 1:
                // industrial
                vibeImages = images[22..26];
                break;
            case 2:
                // coastal
                vibeImages = images[8..13];
                break;
            case 3:
                // vintage
                vibeImages = images[35..40];
                break;
            case 4:
                // cottagecore
                vibeImages = images[13..17];
                break;
            case 5:
                // rustic
                vibeImages = images[30..35];
                break;
            case 6:
                // mid-century modern
                vibeImages = images[26..30];
                break;
            case 7:
                // eclectic
                vibeImages = images[17..22];
                break;
            case 8:
                // art deco
                vibeImages = images[4..8];
                break;
            default:
                vibeImages = images;
                break;
        }
    }

    void CreateImageObjects()
    {
        DestroyImageObjects();

        // Now, you have the correct length for vibeImages
        imageObjects = new GameObject[vibeImages.Length];

        for (int i = 0; i < vibeImages.Length; i++)
        {
            GameObject imageObject = Instantiate(imagePrefab, transform);
            RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
            rectTransform.anchoredPosition = new Vector2(-50f + i * (imageWidth - 10f), 225f);

            Image imageComponent = imageObject.GetComponent<Image>();
            imageComponent.sprite = vibeImages[i];

            imageObjects[i] = imageObject;
        }
    }

    void DestroyImageObjects()
    {
        if (imageObjects != null)
        {
            for (int i = 0; i < imageObjects.Length; i++)
            {
                Destroy(imageObjects[i]);
            }
        }
    }

    private void Update()
    {
        // Check for changes in the selected vibe index and update vibeImages accordingly
        if (PlayerPrefs.GetInt("SelectedVibeIndex", 0) != previousSelectedIndex)
        {
            UpdateVibeImages();
            CreateImageObjects();
            previousSelectedIndex = PlayerPrefs.GetInt("SelectedVibeIndex", 0);
        }

        MoveImages();
    }

    void MoveImages()
    {
        for (int i = 0; i < vibeImages.Length; i++)
        {
            RectTransform rectTransform = imageObjects[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition += Vector2.right * imageSpeed * Time.deltaTime;

            // Check if the image is completely off the screen
            if (rectTransform.anchoredPosition.x > -50f + (vibeImages.Length - 1) * (imageWidth - 10f))
            {
                // Move the image back to the starting position
                rectTransform.anchoredPosition = new Vector2(-50f - (imageWidth - 10f), 225f);
            }
        }
    }
}
