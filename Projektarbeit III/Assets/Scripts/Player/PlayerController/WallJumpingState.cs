using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private float jumpForceUp;
    private float jumpForceSide;
    private float moveSpeed;
    private bool jumpingFromLeftWall;
    private float wallJumpCooldown;
    private float wallJumpCooldownTimer;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower;

    public WallJumpingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        jumpForceUp = controller.movementEditor.wallJumpForce;
        jumpForceSide = controller.movementEditor.wallJumpSideForce;
        moveSpeed = controller.movementEditor.moveSpeed;
        wallJumpCooldown = controller.movementEditor.wallJumpCooldown;
        wallJumpingPower = new Vector2(jumpForceSide, jumpForceUp);
    }

    public void OnEnter()
    {
        Debug.Log("Entered Wall Jumping State");

        // Determine which wall the player is jumping from
        jumpingFromLeftWall = controller.IsTouchingLeftWall();

        // Apply the jump force in the opposite direction of the wall
        wallJumpingDirection = jumpingFromLeftWall ? 1 : -1;
        rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

        controller.movementEditor.hasJumped = true; // Set the jump flag
        wallJumpCooldownTimer = wallJumpCooldown; // Initialize the cooldown timer
        wallJumpingCounter = wallJumpingTime; // Initialize the wall jumping counter
    }

    public void UpdateState()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>();

        // Allow player to influence horizontal movement while maintaining momentum
        if (wallJumpingCounter > 0)
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        else
        {
            MovementUtils.ApplyHorizontalMovement(rb, movementAction, moveSpeed);

            if (rb.linearVelocity.x > 0)
            {
                controller.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (rb.linearVelocity.x < 0)
            {
                controller.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

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
