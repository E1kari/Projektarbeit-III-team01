using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State indexing", menuName = "Scriptable Objects/State indexing")]
public class StateIndexingBecauseTheAnimatorIsMean : ScriptableObject
{
    Dictionary<string, int> playerStates = new Dictionary<string, int>();

    public void init()
    {
        Debug.LogWarning("State indexing because the animator is mean");
        playerStates["IdleState"] = 0;
        playerStates["WalkingState"] = 1;
        playerStates["JumpingState"] = 2;
        playerStates["WallJumpingState"] = 3;
        playerStates["WallStickingState"] = 4;
        playerStates["DashingState"] = 5;
        playerStates["GrapplingState"] = 6;
    }
    public int GetPlayerIndex(string stateName)
    {
        return playerStates[stateName];
    }
}
