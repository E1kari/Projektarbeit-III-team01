using UnityEngine;
using UnityEngine.InputSystem;

public class WallStickingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private float stickDuration;
    private float stickTimer;
    private bool stickingToLeftWall;

    public WallStickingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        stickDuration = controller.movementEditor.stickDuration;
        stickTimer = stickDuration;
        controller.movementEditor.hasJumped = false;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Wall Sticking State");

        // Determine which wall the player is sticking to
        stickingToLeftWall = controller.IsTouchingLeftWall();
        rb.linearVelocity = Vector2.zero; // Stop movement initially
        rb.gravityScale = 0f; // Disable gravity
        stickTimer = stickDuration; // Initialize the stick timer
    }

    public void UpdateState()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>();
        stickTimer -= Time.deltaTime;

        // Ensure the player presses the correct input for sticking
        bool correctInput = stickingToLeftWall ? movementInput.x < 0 : movementInput.x > 0;

        // Transition to IdleState if the player is grounded or input is invalid
        if (controller.IsGrounded() || !correctInput || (!controller.IsTouchingLeftWall() && !controller.IsTouchingRightWall()))
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