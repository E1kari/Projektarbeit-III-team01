using System.Collections;
using UnityEngine;

public class EnemyAttackState : Interface.IState
{
    private Enemy enemy;
    bool fired = false;

    public EnemyAttackState(Enemy pa_enemy)
    {
        enemy = pa_enemy;
    }

    public void OnEnter()
    {
        enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        Debug.Log("Enemy entered Attack State");
    }

    public void UpdateState()
    {
        if (!fired)
        {
            fired = true;
            enemy.StartCoroutine(attack());
        }
        CheckExitConditions();
    }

    protected IEnumerator attack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, player.transform.position - enemy.transform.position);
        if (hit && hit.collider.gameObject.tag == "Player")
        {
            GameObject newProjectile = GameObject.Instantiate(enemy.lightEnemyData_.projectilePrefab_, enemy.transform.position, enemy.transform.rotation);
            newProjectile.GetComponent<Projectile>().init(enemy.lightEnemyData_.projectileSpeed_, enemy.lightEnemyData_.projectileSize_, enemy.gameObject);
        }
        yield return null;
    }

    public void CheckExitConditions()
    {
        // Check for colliders overlapping with a circle
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.lightEnemyData_.attackRange_);
        bool playerFound = false;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerFound = true;
                break;
            }
        }

        if (!playerFound)
        {
            Debug.Log("Player lost. Enemy changing to Idle State");
            enemy.ChangeState(new EnemyIdleState(enemy));
        }
        else
        {
            Debug.Log("Player still in range. Enemy changing to Alert State");
            enemy.ChangeState(new EnemyAlertState(enemy, Time.time));
        }

    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }
}
