using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool startInGrapple = false;

    public S_Light_Enemy lightEnemyData_;

    private Interface.IState currentState;

    public string currentStateName;
    private int lastStateIndex = 0;
    private Animator animator;

    private StateIndexingBecauseTheAnimatorIsMean stateIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        stateIndex = Resources.Load<StateIndexingBecauseTheAnimatorIsMean>("Scriptable Objects/State indexing");
        stateIndex.init();

        lightEnemyData_ = Resources.Load<S_Light_Enemy>("Scriptable Objects/S_Light_Enemy");
        currentState = new EnemyIdleState(this);
        currentStateName = currentState.GetType().Name;
        if (startInGrapple)
        {
            ChangeState(new EnemyGrappledState(this));
        }
        UpdateEnemyAnimator();
    }

    public void ChangeState(Interface.IState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState?.OnEnter();
        currentStateName = currentState.GetType().Name;
        UpdateEnemyAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void UpdateEnemyAnimator()
    {
        if (animator == null)
        {
            //Debug.Log("Animator component is missing on the Enemy game object.");
            return;
        }
        int enemyIndex = stateIndex?.GetEnemyIndex(currentState?.GetType().Name) ?? -1;

        if (enemyIndex != lastStateIndex)
        {
            lastStateIndex = enemyIndex;

            animator.SetInteger("State", enemyIndex);
            animator.SetTrigger("switch");
        }
    }
}
