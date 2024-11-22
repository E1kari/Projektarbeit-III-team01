using System.Collections;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Light_Enemy : B_Enemy
{
    [SerializeField]
    protected S_Light_Enemy lightEnemyData_;

    [SerializeField]
    protected bool drawRange_;
    protected CircleCollider2D attackRange_;


    void Start()
    {
        attackRange_ = gameObject.AddComponent<CircleCollider2D>();
        attackRange_.radius = lightEnemyData_.attackRange_;
        attackRange_.isTrigger = true;
        attackCooldown_ = lightEnemyData_.attackCooldown_;
    }

    override protected IEnumerator attack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (hit && hit.collider.gameObject.tag == "Player")
        {
            GameObject newProjectile = Instantiate(lightEnemyData_.projectilePrefab_, transform.position, transform.rotation);
            newProjectile.GetComponent<Projectile>().init(lightEnemyData_.projectileSpeed_, lightEnemyData_.projectileSize_, gameObject);

            lastAttack_ = Time.time;
        }
        yield return null;
    }

    void OnDrawGizmos()
    {
        if (drawRange_)
        {
            Gizmos.color = new Color(1.0f, 0.65f, 0.85f, 1.0f);
            Gizmos.DrawWireSphere(transform.position, lightEnemyData_.attackRange_);
        }
    }

    void OnDrawGizmosSelected()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            return;
        }
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
