using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private float jumpForceUp;

    private float jumpForceSide;
    private float moveSpeed;
    private bool jumpingFromLeftWall;
    private float wallJumpCooldown;
    private float wallJumpCooldownTimer;

    public WallJumpingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpForceUp = controller.movementEditor.wallJumpForce;
        jumpForceSide = controller.movementEditor.wallJumpForce;
        moveSpeed = controller.movementEditor.moveSpeed;
        wallJumpCooldown = controller.movementEditor.wallJumpCooldown;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Wall Jumping State");

        // Determine which wall the player is jumping from
        jumpingFromLeftWall = controller.IsTouchingLeftWall();

        // Apply the jump force in the opposite direction of the wall
        Vector2 jumpDirection = jumpingFromLeftWall ? new Vector2(1, 1).normalized : new Vector2(-1, 1).normalized;
        Debug.Log("Jumping from " + (jumpingFromLeftWall ? "left" : "right") + " wall");
        rb.linearVelocity = new Vector2(jumpDirection.x * jumpForceSide, jumpDirection.y * jumpForceUp); // Apply the jump force

        controller.movementEditor.hasJumped = true; // Set the jump flag
        wallJumpCooldownTimer = wallJumpCooldown; // Initialize the cooldown timer
    }

    public void UpdateState()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>();

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

        // Decrease the cooldown timer
        wallJumpCooldownTimer -= Time.deltaTime;

        // Transition to IdleState when the player starts falling and is grounded
        if (rb.linearVelocity.y <= 0 && controller.IsGrounded())
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Check for wall and ceiling collisions
        if (wallJumpCooldownTimer <= 0 && controller.IsWalkingAgainstWall())
        {
            Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(new WallStickingState(controller));
        }

        if (controller.IsCeilinged())
        {
            Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new IdleState(controller));
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        Debug.Log("Exiting Wall Jumping State");
    }
}
