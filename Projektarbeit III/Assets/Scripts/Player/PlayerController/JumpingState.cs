using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float jumpForce;
    private float fallForce;
    private float moveSpeed;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction dashAction;
    private float wallJumpCooldownTimer;

    public JumpingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        dashAction = playerInput.actions["Dashing"];
        moveSpeed = controller.movementEditor.moveSpeed;
        jumpForce = controller.movementEditor.jumpForce;
        fallForce = controller.movementEditor.fallForce;
        wallJumpCooldownTimer = 0f;
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Jumping State");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply upward force

        // Reset the wall jump cooldown timer if the player was previously wall jumping
        if (controller.GetPreviousState() is WallJumpingState)
        {
            wallJumpCooldownTimer = 0.5f;
        }
        else
        {
            wallJumpCooldownTimer = 0f;
        }
    }

    public void UpdateState()
    {
        MovementUtils.ApplyHorizontalMovement(rb, movementAction, moveSpeed, controller.movementEditor.maxSpeed);

        // Apply gravitational pull after reaching the peak of the jump
        if (!(rb.linearVelocity.y >= 0))
        {
            controller.ChangeState(new FallingState(controller));
        }

        // Transition to IdleState when the player stops falling and is grounded
        if (rb.linearVelocity.y <= 0 && controller.IsGrounded())
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Transition to DashingState if Dash button is pressed and player hasn't dashed yet
        if (dashAction.triggered && !controller.movementEditor.hasDashed)
        {
            if (controller.IsGrounded())
            {
                //Debug.Log("Player is grounded");
                //Debug.Log("Cannot dash while grounded");
            }
            else
            {
                controller.ChangeState(new DashingState(controller));
                controller.movementEditor.hasDashed = true;
            }
        }

        // Check for wall and ceiling collisions
        WallStickingState wallStickingState = new WallStickingState(controller);
        if (wallStickingState.StickingCheck() && wallJumpCooldownTimer <= 0)
        {
            //Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(wallStickingState);
        }

        if (controller.IsCeilinged())
        {
            //Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new FallingState(controller));
        }

        // Decrease the wall jump cooldown timer
        if (wallJumpCooldownTimer > 0)
        {
            wallJumpCooldownTimer -= Time.deltaTime;
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Jumping State");
    }
}