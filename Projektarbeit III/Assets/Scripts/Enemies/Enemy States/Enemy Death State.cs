using UnityEngine;


public class EnemyDeathState : Interface.IState
{
    private Enemy enemy;

    public EnemyDeathState(Enemy pa_enemy)
    {
        enemy = pa_enemy;
        enemy.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void OnEnter()
    {
        Debug.LogWarning("Enemy entered Death State");
        enemy.gameObject.GetComponent<Health>()?.takeDamage();
    }

    public void UpdateState()
    {

    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }

}
