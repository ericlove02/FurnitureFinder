using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeGalleryNamespace;
using TMPro;

public class ExportHandler : MonoBehaviour
{
    [SerializeField] private GameObject imageDisplayPanel;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject imageExportPanel;
    [SerializeField] private Image imageExport;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button shareButton;

    public void CaptureImage()
    {
        ScreenCapture.CaptureScreenshot("export.png");
        StartCoroutine(DisplayScreenshot());
    }

    private IEnumerator DisplayScreenshot()
    {
        yield return new WaitForSeconds(1f);

        string screenshotPath = Application.persistentDataPath + "/export.png";

        if (System.IO.File.Exists(screenshotPath))
        {
            imageExportPanel.SetActive(true);
            imageExportPanel.transform.localPosition = Vector3.zero;

            Texture2D texture = NativeGallery.LoadImageAtPath(screenshotPath, 512);
            if (texture != null)
            {
                imageExport.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
        else
        {
            Debug.LogWarning("Screenshot file not found.");
        }
    }

    // Coroutine to save screenshot to device after a delay
    public void SaveScreenshot()
    {
        string screenshotPath = Application.persistentDataPath + "/export.png";

        if (System.IO.File.Exists(screenshotPath))
        {
            NativeGallery.SaveImageToGallery(screenshotPath, "MyGallery", "chAiR.png");

            imageExportPanel.SetActive(false);
            imageExportPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);

            imageDisplayPanel.SetActive(true);
            imageDisplayPanel.transform.localPosition = Vector3.zero;

            Texture2D texture = NativeGallery.LoadImageAtPath(screenshotPath, 512);
            if (texture != null)
            {
                imageDisplay.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
        else
        {
            Debug.LogWarning("Screenshot file not found.");
        }
    }

    public void ClosePanel()
    {
        imageDisplayPanel.SetActive(false);
        imageDisplayPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);
    }

    public void ShareImage()
    {
        // StartCoroutine (ShareImageInAndroid());

        string screenshotPath = Application.persistentDataPath + "/export.png";

		if (!Application.isEditor) {
            new NativeShare().AddFile(screenshotPath).Share();
        }
        imageExportPanel.SetActive(false);
        imageExportPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);
    }

    public void CancelExport()
    {
        imageExportPanel.SetActive(false);
        imageExportPanel.transform.localPosition = new Vector3(9999f, 0f, 0f);
    }
}
