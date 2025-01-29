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
        Debug.Log("Enemy entered Idle State");
    }

    public void UpdateState()
    {
        CheckExitConditions();
    }

    public void CheckExitConditions()
    {
        // Check for alert state
        float attackRange = enemy.lightEnemyData_.attackRange_;
        // Check for colliders overlapping with a circle
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.transform.position, attackRange);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player found. Enemy changing to Alert State");
                enemy.ChangeState(new EnemyAlertState(enemy, Time.time));
                return;
            }
        }

        // Check for falling state
        SpriteRenderer spriteRenderer = enemy.gameObject.GetComponent<SpriteRenderer>();
        float spriteHeight = spriteRenderer.bounds.size.y / 2;

        Vector2 raycastStart = (Vector2)enemy.gameObject.transform.position - new Vector2(0, spriteHeight);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, enemy.lightEnemyData_.fallDistance_, LayerMask.GetMask("Ground"));

        if (!hit)
        {
            Debug.Log("Enemy is falling. Enemy changing to Falling State");
            enemy.ChangeState(new EnemyFallingState(enemy));
        }
    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }

}
