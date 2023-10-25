using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetOrientation : MonoBehaviour
{
    public bool isLandscape = false;

    // Set orientation for scene
    void Start()
    {
        if (isLandscape)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
