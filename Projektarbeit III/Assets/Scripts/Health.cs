using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public void takeDamage()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject, 0.1f);
    }
}
