using System.Collections;
using UnityEngine;


public enum EnemyState
{
    IDLE,
    ATTACK,
    GRAPPLED,
}

abstract public class B_Enemy : MonoBehaviour
{
    protected Health health_;
    protected EnemyState enemyState_ = EnemyState.IDLE;
    protected float attackCooldown_;
    protected float lastAttack_;

    void Start()
    {
        health_ = gameObject.AddComponent<Health>();
        lastAttack_ = Time.time;
    }

    void Update()
    {
        if (enemyState_ == EnemyState.ATTACK)
        {
            if (Time.time - lastAttack_ > attackCooldown_)
            {
                StartCoroutine(attack());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            enemyState_ = EnemyState.ATTACK;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            enemyState_ = EnemyState.IDLE;
        }
    }

    abstract protected IEnumerator attack();
}
