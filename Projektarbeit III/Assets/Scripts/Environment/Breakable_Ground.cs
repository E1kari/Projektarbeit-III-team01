using System.Collections;
using UnityEngine;

public class Breakable_Ground : MonoBehaviour
{
    [HideInInspector]
    public S_Breakable_Ground_Data breakableGroundData_;
    private BoxCollider2D boxCollider2D_;
    private BoxCollider2D triggerCollider2D_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider2D_ = gameObject.AddComponent<BoxCollider2D>();
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        boxCollider2D_.size = new Vector2(spriteSize.x - breakableGroundData_.graceArea_, spriteSize.y - breakableGroundData_.graceArea_);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other.contacts[0].normal + " " + Vector2.down);
            if (other.contacts[0].normal == Vector2.down)
            {
                StartCoroutine(activateBreaking());
            }
        }
    }

    IEnumerator activateBreaking()
    {
        yield return new WaitForSeconds(breakableGroundData_.breakDelay_);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.DrawWireCube(transform.position, new Vector2(spriteSize.x - breakableGroundData_.graceArea_, spriteSize.y - breakableGroundData_.graceArea_));
    }
}