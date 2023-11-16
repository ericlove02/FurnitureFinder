using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityWebRequest = UnityEngine.Networking.UnityWebRequest;

public class PrintPrices : MonoBehaviour
{
    public string apiUrl = "https://hammy-exchanges.000webhostapp.com/index.php?vibe_name=industrial";
    public TextMeshPro textMeshPro; // Reference to a TextMeshPro component
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // display the response on the 3D text
                textMeshPro.text = "Received: " + webRequest.downloadHandler.text;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
