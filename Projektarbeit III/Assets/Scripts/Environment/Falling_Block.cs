using System.Collections;
using UnityEngine;

public class Falling_Block : MonoBehaviour
{
    [HideInInspector]
    public S_Falling_Block fallingBlockData_;

    private BoxCollider2D boxCollider2D_;
    private Rigidbody2D rb2D_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D_ = gameObject.AddComponent<Rigidbody2D>();
        rb2D_.gravityScale = 0.0f;
        rb2D_.freezeRotation = true;
        boxCollider2D_ = gameObject.AddComponent<BoxCollider2D>();
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        boxCollider2D_.size = spriteSize;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb2D_.gravityScale = fallingBlockData_.gravityScale_;
        }
    }
}
