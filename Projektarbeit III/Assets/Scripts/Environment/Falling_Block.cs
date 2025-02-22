using System.Collections;
using UnityEngine;

public class Falling_Block : MonoBehaviour
{
    [HideInInspector]
    public S_Falling_Block fallingBlockData_;

    private bool countdown = false;
    private bool triggered = false;
    private SpriteRenderer spriteRenderer;

    private BoxCollider2D boxCollider2D_;
    private BoxCollider2D boxTrigger2D_;
    private Rigidbody2D rb2D_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D_ = gameObject.AddComponent<Rigidbody2D>();
        rb2D_.constraints = RigidbodyConstraints2D.FreezePosition;
        rb2D_.freezeRotation = true;

        boxCollider2D_ = gameObject.AddComponent<BoxCollider2D>();
        Vector2 spriteSize = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.size;
        boxCollider2D_.size = spriteSize;

        boxTrigger2D_ = gameObject.AddComponent<BoxCollider2D>();
        boxTrigger2D_.size = spriteSize + new Vector2(fallingBlockData_.graceDistance_, fallingBlockData_.graceDistance_);
        boxTrigger2D_.isTrigger = true;

        spriteRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.gameObject.tag == "Player")
        {
            StartCoroutine(startCountdown());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            activateGravity();
        }
    }

    IEnumerator startCountdown()
    {
        countdown = true;
        triggered = true;
        StartCoroutine(startShaking());
        yield return new WaitForSeconds(fallingBlockData_.fallDelay_);
        restetSpritePosition();
        countdown = false;
        activateGravity();
    }

    IEnumerator startShaking()
    {
        int i = 0;
        float j = 0.01f;
        while (true)
        {
            if (!countdown)
            {
                yield break;
            }
            i++;

            if (i % 10 == 0)
            {
                j *= -1;
            }

            spriteRenderer.gameObject.transform.position = new Vector2(spriteRenderer.gameObject.transform.position.x + j, spriteRenderer.gameObject.transform.position.y);
            yield return null;
        }
    }

    private void restetSpritePosition()
    {
        spriteRenderer.gameObject.transform.localPosition = new Vector2(0, 0);
    }

    private void activateGravity()
    {
        rb2D_.constraints = ~RigidbodyConstraints2D.FreezePositionY;
        rb2D_.gravityScale = fallingBlockData_.gravityScale_;
        rb2D_.sharedMaterial = new PhysicsMaterial2D() { friction = 0, bounciness = 0 };

        // reapplying simulated because unity wont update the rigidbody ;-;
        rb2D_.simulated = false;
        rb2D_.simulated = true;
    }

    void FixedUpdate()
    {
        if (rb2D_.linearVelocity.y > 0)
        {
            rb2D_.linearVelocity = new Vector2(rb2D_.linearVelocity.x, 0);
        }
    }

    void OnDrawGizmos()
    {
        Vector2 spriteSize = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.size;
        spriteSize += new Vector2(fallingBlockData_.graceDistance_, fallingBlockData_.graceDistance_);
        Gizmos.DrawWireCube(transform.position, spriteSize);
    }
}
