using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetType() == typeof(CapsuleCollider2D))
        {
            try
            {
                other.gameObject.GetComponent<Health>().takeDamage();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

}
