using UnityEngine;

public class Camera_Spot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireCube(transform.position, gameObject.GetComponent<SpriteRenderer>().bounds.size);
    }
}
