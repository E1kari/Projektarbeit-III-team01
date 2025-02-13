using Unity.VisualScripting;
using UnityEngine;

public class EnemyAlertState : Interface.IState
{
    private Enemy enemy;
    float lastAttack_;

    public EnemyAlertState(Enemy pa_enemy, float pa_currentTime)
    {
        enemy = pa_enemy;
        lastAttack_ = pa_currentTime;
    }

    public void OnEnter()
    {
        enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        //Debug.Log("Enemy entered Alert State");
    }

    public void UpdateState()
    {
        CheckExitConditions();
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
            //Debug.Log("Player lost. Enemy changing to Idle State");
            enemy.ChangeState(new EnemyIdleState(enemy));
        }
        else
        {
            if (Time.time - lastAttack_ > enemy.lightEnemyData_.attackCooldown_)
            {
                //Debug.Log("Enemy attacking. Enemy changing to Attack State");
                enemy.ChangeState(new EnemyAttackState(enemy));
            }
        }
    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }
}
