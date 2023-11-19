using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject prefab; // Prefab of the object you want to add to the scroll view
    public Transform content; // Content transform of the scroll view
    // public int numberOfObjects = 10; // Number of objects you want to add
    public GameObject[] furnitureItems;
    public Sprite[] furnImages; 

    void Start()
    {
        // Call a method to populate the scroll view with objects
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Loop through the number of objects you want to add
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

        // Calculate the size of the content based on the number of objects and their size
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float contentHeight = furnitureItems.Length * prefab.GetComponent<RectTransform>().rect.height;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }
}
