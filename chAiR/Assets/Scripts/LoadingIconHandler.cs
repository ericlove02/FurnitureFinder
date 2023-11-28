using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconHandler : MonoBehaviour
{
    public float rotationSpeed = -150f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the image icon smoothly around the up axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}