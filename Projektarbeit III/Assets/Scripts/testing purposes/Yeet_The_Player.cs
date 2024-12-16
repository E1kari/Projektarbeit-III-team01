using UnityEngine;

public class Yeet_The_Player : MonoBehaviour
{
    public Transform destination_;

    public float yeetSpeed_ = 10f;
    public bool travelToEnd = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        Vector2 direction;
        if (travelToEnd)
        {
            GameObject end = GameObject.FindGameObjectsWithTag("Level End")[0];
            destination_ = end.transform;

        }
        if (destination_)
        {
            direction = new Vector2(destination_.position.x - transform.position.x, destination_.position.y - transform.position.y + transform.localScale.y).normalized;
        }
        else
        {
            Debug.LogWarning("No destination set for Yeet_The_Player");
            direction = new Vector2(0, 0);
        }
        rb.AddForce(direction * yeetSpeed_, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (travelToEnd)
        {
            GameObject end = GameObject.FindGameObjectsWithTag("Level End")[0];
            if (end)
            {
                destination_ = end.transform;
            }
        }
        if (destination_)
        {
            Gizmos.DrawWireSphere(destination_.position, 0.2f);
            Gizmos.color = Color.green;
            Vector2 feetsies = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
            Gizmos.DrawLine(feetsies, destination_.position);
            return;
        }

        Debug.LogWarning("No destination set for Yeet_The_Player");
        Gizmos.DrawWireSphere(transform.position, 0.2f);

    }

}
