using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State indexing", menuName = "Scriptable Objects/State indexing")]
public class StateIndexingBecauseTheAnimatorIsMean : ScriptableObject
{
    Dictionary<string, int> playerStates = new Dictionary<string, int>();
    Dictionary<string, int> enemyStates = new Dictionary<string, int>();

    public void init()
    {
        playerStates["IdleState"] = 0;
        playerStates["WalkingState"] = 1;
        playerStates["JumpingState"] = 2;
        playerStates["WallJumpingState"] = 3;
        playerStates["WallStickingState"] = 4;
        playerStates["DashingState"] = 5;
        playerStates["GrapplingState"] = 6;
        playerStates["FallingState"] = 7;


        enemyStates["EnemyIdleState"] = 0;
        enemyStates["EnemyFallingState"] = 1;
        enemyStates["EnemyGrappledState"] = 2;
        enemyStates["EnemyAlertState"] = 3;
        enemyStates["EnemyAttackState"] = 4;
        enemyStates["EnemyDeathState"] = 5;
    }
    public int GetPlayerIndex(string stateName)
    {
        return playerStates[stateName];
    }

    public int GetEnemyIndex(string stateName)
    {
        return enemyStates[stateName];
    }
}
