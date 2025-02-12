using UnityEngine;
using UnityEngine.InputSystem;

public class WallStickingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction stickAction;
    private float stickTimer;
    private bool stickingToLeftWall;
    private float wallJumpCooldownTimer;

    public WallStickingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        stickAction = playerInput.actions["WallSticking"];
        controller.movementEditor.hasJumped = false;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Wall Sticking State");

        // Determine which wall the player is sticking to
        stickingToLeftWall = controller.IsTouchingLeftWall();
        rb.linearVelocity = Vector2.zero; // Stop movement initially
        rb.gravityScale = 0f; // Disable gravity
        stickTimer = controller.movementEditor.stickDuration; // Initialize the stick timer
    }

    public void UpdateState()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>();
        stickTimer -= Time.deltaTime;
        wallJumpCooldownTimer -= Time.deltaTime;

        // Ensure the player presses the correct input for sticking
        bool correctInput = stickingToLeftWall ? movementInput.x < 0 : movementInput.x > 0;

        // Transition to IdleState if the player is grounded, input is invalid or WallSticking button is released
        if (controller.IsGrounded() || !correctInput || !stickAction.IsPressed())
        {
            controller.ChangeState(new IdleState(controller));
            return;
        }

        if (stickTimer <= 0)
        {
            Debug.Log("The Stick Timer has expired");
            controller.ChangeState(new IdleState(controller));
            return;
        }

        // Transition to WallJumpingState if Jump button is pressed and player hasn't jumped yet
        if (jumpAction.triggered && !controller.movementEditor.hasJumped)
        {
            controller.ChangeState(new WallJumpingState(controller));
            return;
        }

        // Ensure the player sticks to the wall without sliding down
        rb.linearVelocity = Vector2.zero;
    }

    public bool StickingCheck()
    {
        return controller.IsWalkingAgainstWall() && wallJumpCooldownTimer <= 0 && stickAction.IsPressed();
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        Debug.Log("Exiting Wall Sticking State");

        // Reset gravity scale when exiting the state
        rb.gravityScale = 1f; // Default gravity scale
    }
}