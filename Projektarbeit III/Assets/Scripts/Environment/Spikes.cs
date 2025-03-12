using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().takeDamage();
            return;
        }

        if (other.gameObject.tag == "Light Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.ChangeState(new EnemyDeathState(enemy));
            return;
        }
    }
}
