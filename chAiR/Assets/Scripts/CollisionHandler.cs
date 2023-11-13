using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Triggered when a collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the colliding object

        // Optionally, you can also destroy the current object
        Destroy(gameObject);
    }

    /*
    private void OnTriggerEnter(Collision collision)
    {
        // Destroy the colliding object

        // Optionally, you can also destroy the current object
        Destroy(gameObject);
    }
    */

    private void OnCollisionStay(Collision collision)
    {
        // Destroy the colliding object
        // Optionally, you can also destroy the current object
        Destroy(gameObject);
    }
}