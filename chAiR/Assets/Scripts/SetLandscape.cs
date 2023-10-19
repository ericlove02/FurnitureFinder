using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLandscape : MonoBehaviour
{
    // Start in landscape mode
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

}
