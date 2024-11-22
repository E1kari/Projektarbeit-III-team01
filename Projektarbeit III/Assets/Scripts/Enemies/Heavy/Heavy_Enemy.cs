using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Heavy_Enemy : B_Enemy
{

    [SerializeField]
    protected S_Heavy_Enemy_Data heavyEnemyData_;

    [SerializeField]
    protected bool drawRange_;
    protected CircleCollider2D attackRange_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(heavyEnemyData_.attackRange_);
        attackRange_ = gameObject.AddComponent<CircleCollider2D>();
        attackRange_.radius = heavyEnemyData_.attackRange_ / transform.localScale.x;
        attackRange_.isTrigger = true;
        attackCooldown_ = heavyEnemyData_.attackCooldown_;
    }

    override protected IEnumerator attack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (hit)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.3f, 1.0f, 1.0f);
            yield return new WaitForSeconds(heavyEnemyData_.attackDuration_);
            lastAttack_ = Time.time;
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

            if (GameObject.FindWithTag("Player") && Physics2D.Raycast(transform.position, player.transform.position - transform.position))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (Vector2.Distance(transform.position, player.transform.position) < heavyEnemyData_.attackRange_)
                    {
                        player.GetComponent<Health>().takeDamage();
                    }
                }
            }
        }
        yield return null;
    }

    void OnDrawGizmos()
    {
        if (drawRange_)
        {
            Gizmos.color = new Color(0.65f, 0.85f, 1.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position, heavyEnemyData_.attackRange_);
        }
    }

    void OnDrawGizmosSelected()
    {
        GameObject player = GameObject.FindWithTag("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (hit && hit.collider.gameObject.tag == "Player")
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position, player.transform.position - transform.position);
        Gizmos.DrawLine(hit.point, hit.point + new Vector2(0.0f, 2f));
    }
}
