using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed_;
    private float size_;
    private Rigidbody2D rb_;

    private Vector2 direction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = new Vector3(size_, size_, size_);
        rb_ = GetComponent<Rigidbody2D>();

        rb_.gravityScale = 0.0f;

        GameObject player = GameObject.FindWithTag("Player");


        float xDistance = player.transform.position.x - transform.position.x;
        float yDistance = player.transform.position.y - transform.position.y;

        if (yDistance < xDistance)
        {
            if (yDistance > 1.0f)
            {
                xDistance = xDistance / yDistance;
                yDistance = 1.0f;
            }
            else if (yDistance < -1.0f)
            {
                xDistance = xDistance / yDistance * -1.0f;
                yDistance = -1.0f;
            }
        }
        else
        {
            if (xDistance > 1.0f)
            {
                yDistance = (yDistance / xDistance);
                xDistance = 1.0f;
            }
            else if (xDistance < -1.0f)
            {
                yDistance = (yDistance / xDistance) * -1.0f;
                xDistance = -1.0f;
            }
        }
        direction = new Vector2(xDistance * speed_, yDistance * speed_);
        rb_.AddForce(direction, ForceMode2D.Impulse);
    }

    public void init(float pa_speed, float pa_size, GameObject pa_shootingEnemy)
    {
        speed_ = pa_speed;
        size_ = pa_size;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), pa_shootingEnemy.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, direction);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().takeDamage();
        }
        Destroy(gameObject);
    }

}
