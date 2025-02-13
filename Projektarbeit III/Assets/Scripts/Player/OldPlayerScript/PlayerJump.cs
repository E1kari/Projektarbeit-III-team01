using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight;
    public InputActionReference jump;
    public LayerMask groundLayer;          // Layer mask to specify what is considered ground
    public Transform groundCheck;          // Empty GameObject positioned at the player's feet
    public float groundCheckRadius = 0.2f; // Radius of the ground check

    private PlayerController playerController;
    private bool isGrounded = false;       // Tracks if the player is on the ground
    private bool hasJumped = false;        // Tracks if the player has jumped

    public enum JumpingState
    {
        WIND_UP,
        JUMP,
        LANDING
    }

    public JumpingState jumpingState;

    private void Start()
    {
        rb = rb != null ? rb : GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        jump.action.Enable();
    }

    private void Update()
    {
        // Update grounded status
        isGrounded = IsGrounded();

        // Reset jump if the player is on the ground
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            //Debug.Log("Reset jump");
            if (playerController.playerState == PlayerController.PlayerState.JUMPING)
            {
                playerController.SwitchState(PlayerController.PlayerState.IDLING);
            }
        }

        // Handle jumping input
        if (jump.action.ReadValue<float>() > 0 && !hasJumped)
        {
            StartJump();
        }
    }

    private void StartJump()
    {
        // Transition to the JUMPING state and apply the jump force
        playerController.SwitchState(PlayerController.PlayerState.JUMPING);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight); // Apply upward force
        jumpingState = JumpingState.JUMP;
        hasJumped = true; // Set the jumped flag to true
        //Debug.Log("Jumped");
    }

    public void HandleJumping()
    {
        // Manage jump states
        switch (jumpingState)
        {
            case JumpingState.WIND_UP:
                // Prepare for jump (if needed for animations or delays)
                jumpingState = JumpingState.JUMP;
                break;

            case JumpingState.JUMP:
                // Transition to LANDING when vertical velocity is downward
                if (rb.linearVelocity.y <= 0)
                {
                    jumpingState = JumpingState.LANDING;
                }
                break;

            case JumpingState.LANDING:
                // Transition to IDLING or WALKING when grounded
                if (isGrounded)
                {
                    playerController.SwitchState(PlayerController.PlayerState.IDLING);
                    hasJumped = false;
                    //Debug.Log("Landed");
                }
                break;
        }
    }

    private bool IsGrounded()
    {
        // Check if the groundCheck collider overlaps with any ground layer colliders
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground check in the editor for debugging
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
