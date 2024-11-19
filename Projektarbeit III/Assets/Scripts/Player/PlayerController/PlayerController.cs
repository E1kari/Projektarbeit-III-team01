using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        IDLING,
        WALKING,
        SlIDING,
        JUMPING,
        STICKING,
        DASHING,
        GRAPPLING
    }

    public PlayerState playerState = PlayerState.IDLING;
    public bool falling = false;
    public float verticalMomentum = 0f;
    public float horizontalMomentum = 0f;

    private PlayerBaseMovement playerBaseMovement;
    private PlayerJump playerJump;

    private void Start()
    {
        playerBaseMovement = GetComponent<PlayerBaseMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        bool stateExecuted = ExecuteCurrentState();

        if (falling)
        {
            ExecuteFalling();
        }

        CorrectHighVelocity();
    }

    private bool ExecuteCurrentState()
    {
        switch (playerState)
        {
            case PlayerState.WALKING:
                playerBaseMovement.HandleWalking();
                return true;
            case PlayerState.JUMPING:
                playerJump.HandleJumping();
                return true;
            default:
                return false;
        }
    }

    private bool ExecuteFalling()
    {
        // Implement falling logic here
        verticalMomentum -= 9.81f * Time.deltaTime; // Apply gravity
        return true;
    }

    public void SwitchState(PlayerState newState)
    {
        playerState = newState;
        // Handle state transition logic here
    }

    private void CorrectHighVelocity()
    {
        // Implement velocity correction logic here
        verticalMomentum = Mathf.Clamp(verticalMomentum, -20f, 20f);
        horizontalMomentum = Mathf.Clamp(horizontalMomentum, -20f, 20f);
        //Debug.Log("Vertical Momentum: " + verticalMomentum);
        //Debug.Log("Horizontal Momentum: " + horizontalMomentum);
    }

    /*private bool IsGrounded()
    {
        // Implement ground check logic, e.g., using raycast or collision detection
        return true;
    } */
}