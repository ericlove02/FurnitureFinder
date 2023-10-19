using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetOrientation : MonoBehaviour
{
    // Set orientation for scene
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ARScene") {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        } else {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
