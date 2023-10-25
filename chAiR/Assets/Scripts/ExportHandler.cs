using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeGalleryNamespace;

public class ExportHandler : MonoBehaviour
{
    public void CaptureImage()
    {
        ScreenCapture.CaptureScreenshot("export.png");
        StartCoroutine(SaveScreenshot());
    }

    // Coroutine to save screenshot to device after a delay
    private IEnumerator SaveScreenshot()
    {
        yield return new WaitForSeconds(1f);

        string screenshotPath = Application.persistentDataPath + "/export.png";

        if (System.IO.File.Exists(screenshotPath))
        {
            NativeGallery.SaveImageToGallery(screenshotPath, "MyGallery", "chAiR.png");
        }
        else
        {
            Debug.LogWarning("Screenshot file not found.");
        }
    }
}
