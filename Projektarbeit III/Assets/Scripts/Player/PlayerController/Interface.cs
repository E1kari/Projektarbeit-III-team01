using UnityEngine;

public class Interface : MonoBehaviour
{
    public interface IState
    {
        void OnEnter();       // Called when the state starts
        void UpdateState();   // Called every frame
        void OnDeath();        // Called when the player dies
        void OnExit();        // Called when the state ends
    }
}
