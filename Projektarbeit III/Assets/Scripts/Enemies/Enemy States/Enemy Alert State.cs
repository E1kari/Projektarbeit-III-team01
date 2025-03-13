using Unity.VisualScripting;
using UnityEngine;
using static S_AudioData;

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
            if (!collider.CompareTag("Player"))
            {
                continue;
            }

            RaycastHit2D hit;
            hit = Physics2D.Raycast(enemy.transform.position, collider.transform.position - enemy.transform.position, enemy.lightEnemyData_.attackRange_);

            if (hit && hit.collider.gameObject.tag == "Player")
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
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Enemy_Death);
    }
}
