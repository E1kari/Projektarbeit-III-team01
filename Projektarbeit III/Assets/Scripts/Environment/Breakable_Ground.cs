using System.Collections;
using UnityEngine;
using static S_AudioData;

public class Breakable_Ground : MonoBehaviour
{
    [HideInInspector]
    public S_Breakable_Ground_Data breakableGroundData_;
    private BoxCollider2D boxCollider2D_;
    private bool playerContact = false;

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
            if (other.contacts[0].normal == Vector2.down)
            {
                playerContact = true;
                StartCoroutine(activateBreaking());
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerContact)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator activateBreaking()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Environment_BreakableBlock);

        yield return new WaitForSeconds(breakableGroundData_.breakDelay_);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.DrawWireCube(transform.position, new Vector2(spriteSize.x - breakableGroundData_.graceArea_, spriteSize.y - breakableGroundData_.graceArea_));
    }
}
