using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{

    private bool takingDamage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (takingDamage)
        {
            StartCoroutine(damageIndicator());
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void takeDamage(int damage)
    {
        takingDamage = true;
    }

    public void stopDamage()
    {
        takingDamage = false;
    }

    IEnumerator damageIndicator()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.0f);
    }
}
