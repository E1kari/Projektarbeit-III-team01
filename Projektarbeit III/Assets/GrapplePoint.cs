using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
