using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public S_Light_Enemy lightEnemyData_;

    private Interface.IState currentState;

    public string currentStateName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightEnemyData_ = Resources.Load<S_Light_Enemy>("Scriptable Objects/S_Light_Enemy");
        currentState = new EnemyIdleState(this);
    }

    public void ChangeState(Interface.IState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState?.OnEnter();
        currentStateName = currentState.GetType().Name;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }
}
