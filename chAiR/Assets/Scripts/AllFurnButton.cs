using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClickScript : MonoBehaviour
{
    // Set the scene name you want to load in the inspector
    public string sceneToLoad;

    void Start()
    {
        // Attach the method to the button's onClick event
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
            int.TryParse(id, out SelectedFurniture.clickedFurnitureId);
            SelectedFurniture.previousPage = "AllFurn";

            // Print the static variable for verification
            Debug.Log("Name: " + SelectedFurniture.clickedFurnitureName);
            Debug.Log("ID: " + SelectedFurniture.clickedFurnitureId);

            // Change the scene
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Text component not found among the children.");
        }
    }
}
