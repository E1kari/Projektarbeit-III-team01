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
        //Debug.Log("Enemy entered Falling State");
    }

    public void UpdateState()
    {
        CheckExitConditions();
        moveOutOfCollision();
    }

    public void CheckExitConditions()
    {
        SpriteRenderer spriteRenderer = enemy.gameObject.GetComponent<SpriteRenderer>();
        float spriteHeight = spriteRenderer.bounds.size.y / 2;

        Vector2 raycastStart = (Vector2)enemy.gameObject.transform.position - new Vector2(0, spriteHeight);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, enemy.lightEnemyData_.fallDistance_, LayerMask.GetMask("Ground"));

        if (!hit)
        {
            return;
        }

        if (hit.collider.gameObject.GetComponent<Spikes>() != null)
        {
            enemy.ChangeState(new EnemyDeathState(enemy));
            return;
        }

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }
    }

    private void moveOutOfCollision()
    {
        float widthStep = enemy.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float heightStep = enemy.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector2 upperCenter = new Vector2(enemy.transform.position.x, enemy.transform.position.y + heightStep);

        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(upperCenter.x, upperCenter.y - (i * heightStep)), Vector2.right, widthStep + 0.1f, LayerMask.GetMask("Ground"));
            if (hit)
            {
                enemy.transform.position = new Vector2(hit.point.x - (widthStep + 0.2f), hit.point.y);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(upperCenter.x, upperCenter.y - (i * heightStep)), Vector2.left, widthStep + 0.1f, LayerMask.GetMask("Ground"));
            if (hit)
            {
                enemy.transform.position = new Vector2(hit.point.x + (widthStep + 0.2f), hit.point.y);
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
