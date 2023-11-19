using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject prefab; // Prefab of the object you want to add to the scroll view
    public Transform content; // Content transform of the scroll view
    // public int numberOfObjects = 10; // Number of objects you want to add
    public GameObject[] furnitureItems;

    void Start()
    {
        // Call a method to populate the scroll view with objects
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Loop through the number of objects you want to add
        foreach(GameObject furniture in furnitureItems)
        {
            GameObject newObject = Instantiate(prefab, content);
            Text textComponent = newObject.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = furniture.name;
            }
        }

        // Calculate the size of the content based on the number of objects and their size
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float contentHeight = furnitureItems.Length * prefab.GetComponent<RectTransform>().rect.height;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }
}
