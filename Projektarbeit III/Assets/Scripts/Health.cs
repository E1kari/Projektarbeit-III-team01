using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private bool invincible = false;

    public virtual void takeDamage()
    {
        if (invincible) return;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        if (gameObject.tag == "Player")
        {
            GameObject levelStart = GameObject.FindWithTag("Level Start");
            levelStart.GetComponent<Level_Start>().spawnPlayer();
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
