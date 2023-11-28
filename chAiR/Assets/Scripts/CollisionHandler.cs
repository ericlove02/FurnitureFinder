using UnityEngine;
using cakeslice;

public class CollisionHandler : MonoBehaviour
{
    private Outline[] outlines;

    void Start()
    {
        // check for outline in parent
        Outline pOutline = GetComponent<Outline>();

        // if outline not in parent, check in children
        if (pOutline != null)
        {
            outlines = new Outline[] { pOutline };
        }
        else
        {
            outlines = GetComponentsInChildren<Outline>();
        }

        // start outline disabled
        foreach (var outline in outlines)
        {
            outline.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Furniture")
        {
            foreach (var outline in outlines)
            {
                outline.enabled = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Furniture")
        {
            foreach (var outline in outlines)
            {
                outline.enabled = false;
            }
        }
    }
}
