using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject, 0.1f);
    }
}
