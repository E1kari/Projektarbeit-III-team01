using UnityEngine;

public class Yeet_The_Player : MonoBehaviour
{
    public Transform destination_;

    public float yeetSpeed_ = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        Vector2 direction = new Vector2(destination_.position.x - transform.position.x, destination_.position.y - transform.position.y + transform.localScale.y).normalized;
        rb.AddForce(direction * yeetSpeed_, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (destination_)
        {
            Gizmos.DrawWireSphere(destination_.position, 0.2f);
            Gizmos.color = Color.green;
            Vector2 feetsies = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
            Gizmos.DrawLine(feetsies, destination_.position);
        }
        else
        {
            Debug.LogWarning("No destination set for Yeet_The_Player");
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }

}
