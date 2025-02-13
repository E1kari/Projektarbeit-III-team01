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
        Vector2 distance = player.transform.position - transform.position;
        Vector2 normal = distance.normalized;

        transform.right = normal * -1.0f;

        rb_.linearVelocity = transform.right * -1 * speed_;
    }

    public void init(float pa_speed, float pa_size, GameObject pa_shootingEnemy)
    {
        speed_ = pa_speed;
        size_ = pa_size;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), pa_shootingEnemy.GetComponent<Collider2D>());
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
