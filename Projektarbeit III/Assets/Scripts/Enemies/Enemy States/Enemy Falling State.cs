using UnityEngine;

public class EnemyFallingState : Interface.IState
{
    private Enemy enemy;

    public EnemyFallingState(Enemy pa_enemy)
    {
        enemy = pa_enemy;
        enemy.gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
    }

    public void OnEnter()
    {
        enemy.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1.0f);
        Debug.Log("Enemy entered Falling State");
    }

    public void UpdateState()
    {
        CheckExitConditions();
    }

    public void CheckExitConditions()
    {
        SpriteRenderer spriteRenderer = enemy.gameObject.GetComponent<SpriteRenderer>();
        float spriteHeight = spriteRenderer.bounds.size.y / 2;

        Vector2 raycastStart = (Vector2)enemy.gameObject.transform.position - new Vector2(0, spriteHeight);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, enemy.lightEnemyData_.fallDistance_, LayerMask.GetMask("Ground"));

        if (hit)
        {
            Debug.Log("Enemy landed. Enemy changing to Idle State");
            enemy.ChangeState(new EnemyIdleState(enemy));
        }
    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }
}
