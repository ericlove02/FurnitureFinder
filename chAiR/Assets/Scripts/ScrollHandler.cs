using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject[] prefab; // Prefab of the object you want to add to the scroll view
    public Transform content; // Content transform of the scroll view
    public int numberOfObjects = 10; // Number of objects you want to add

    void Start()
    {
        // Call a method to populate the scroll view with objects
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Loop through the number of objects you want to add
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Instantiate the prefab
            GameObject newObject = Instantiate(prefab, content);

            // You can customize the newObject properties here if needed
            // For example, set the text of a Text component if your prefab has one
        }

        // Calculate the size of the content based on the number of objects and their size
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float contentHeight = numberOfObjects * prefab.GetComponent<RectTransform>().rect.height;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }
}