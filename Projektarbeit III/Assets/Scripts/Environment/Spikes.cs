using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
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
