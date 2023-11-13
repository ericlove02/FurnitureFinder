using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Triggered when a collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the colliding object
        if(collision.gameObject.tag == "Furniture")
        {
            // make red outline happen
        }
    }

    private void OnCollisonExit(Collision collision)
    {
        if(collision.gameObject.tag == Furniture)
        {
            // make red outline go away
        }
    }
}