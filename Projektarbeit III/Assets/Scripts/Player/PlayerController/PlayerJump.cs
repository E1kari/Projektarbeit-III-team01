using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight;
    public InputActionReference jump;

    private PlayerController playerController;

    public enum JumpingState
    {
        WIND_UP,
        JUMP,
        LANDING
    }

    public JumpingState jumpingState;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        jump.action.Enable();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // Allow jumping only if player is idling or walking
        if (jump.action.ReadValue<float>() > 0 &&
            (playerController.playerState == PlayerController.PlayerState.IDLING 
            || playerController.playerState == PlayerController.PlayerState.WALKING) 
            && IsGrounded())
        {
            StartJump();
        }
    }

    private void StartJump()
    {
        playerController.SwitchState(PlayerController.PlayerState.JUMPING);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    public void HandleJumping()
    {
        switch (jumpingState)
        {
            case JumpingState.WIND_UP:
                // Prepare for jump, set initial jump velocity
                jumpingState = JumpingState.JUMP;
                break;
            case JumpingState.JUMP:
                // Apply upward momentum or jump force
                if (rb.linearVelocity.y <= 0) // Start falling when jump velocity reaches zero
                {
                    jumpingState = JumpingState.LANDING;
                }
                break;
            case JumpingState.LANDING:
                // Transition to IDLING or WALKING once landed
                if (IsGrounded()) // Check if grounded (needs implementation)
                {
                    playerController.SwitchState(PlayerController.PlayerState.IDLING);
                }
                break;
        }
    }

    private bool IsGrounded()
    {
        // Implement ground check logic, e.g., using raycast or collision detection
        return true;
    }
}
