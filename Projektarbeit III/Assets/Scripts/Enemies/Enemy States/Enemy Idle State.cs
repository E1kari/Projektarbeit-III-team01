using UnityEngine;


public class EnemyIdleState : Interface.IState
{
    private Enemy enemy;

    public EnemyIdleState(Enemy pa_enemy)
    {
        enemy = pa_enemy;
        enemy.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void OnEnter()
    {
        enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void UpdateState()
    {
        CheckExitConditions();
    }

    public void CheckExitConditions()
    {
        float attackRange = enemy.lightEnemyData_.attackRange_;
        // Check for colliders overlapping with a circle
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.transform.position, attackRange);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                enemy.ChangeState(new EnemyAlertState(enemy, Time.time));
                return;
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
