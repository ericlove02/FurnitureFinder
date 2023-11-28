/*
using UnityEngine;
using UnityEditor;

public class PrefabToImageConverter : MonoBehaviour
{
    public GameObject[] prefabs;
    public string outputPath = "Assets/Furniture Images/";

    void Start()
    {
        foreach (GameObject prefab in prefabs)
        {
            // Generate a preview texture for the prefab
            Texture2D previewTexture = AssetPreview.GetAssetPreview(prefab);

            // Create a new Texture2D to copy the preview texture
            Texture2D screenshot = new Texture2D(previewTexture.width, previewTexture.height, TextureFormat.RGB24, false);
            screenshot.SetPixels(previewTexture.GetPixels());
            screenshot.Apply();

            // Convert to PNG and save to the Assets folder
            byte[] bytes = screenshot.EncodeToPNG();
            System.IO.File.WriteAllBytes(outputPath + prefab.name + ".png", bytes);
            Debug.Log("Saved" + prefab.name + " to " + outputPath + prefab.name + ".png");
        }
    }
}
*/