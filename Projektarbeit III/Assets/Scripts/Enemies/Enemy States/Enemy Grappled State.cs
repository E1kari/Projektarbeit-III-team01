using UnityEngine;

public class EnemyGrappledState : Interface.IState
{
    private Enemy enemy;
    Vector2 distanceToPlayer;

    public EnemyGrappledState(Enemy pa_enemy)
    {
        enemy = pa_enemy;
    }

    public void OnEnter()
    {
        enemy.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0.5f);
        enemy.gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePosition;
        Debug.Log("Enemy entered Grappled State");
    }

    public void UpdateState()
    {
        GameObject player = GameObject.FindWithTag("Player");
        distanceToPlayer = player.transform.position - enemy.transform.position; // Pulls the enemy towards the player
        enemy.gameObject.GetComponent<Rigidbody2D>().linearVelocity = distanceToPlayer.normalized * player.GetComponent<MovementEditor>().grappleSpeed;
    }

    public void CheckExitConditions()
    {

    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }
}
