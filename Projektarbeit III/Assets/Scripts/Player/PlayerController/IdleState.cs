using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float fallForce;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private float moveSpeed;

    public IdleState(Controller controller)
    {
        this.controller = controller;
        rb = controller.gameObject.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        dashAction = playerInput.actions["Dashing"];
        fallForce = controller.movementEditor.fallForce;
        moveSpeed = controller.movementEditor.moveSpeed;
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Idle State");
        // Reset animations, stop movement, etc.
    }

    public void UpdateState()
    {
        // Example: Transition to WalkingState if horizontal input is detected
        MovementUtils.ApplyHorizontalMovement(rb, movementAction, moveSpeed, controller.movementEditor.maxSpeed);

        // Apply fall force when the player starts falling
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.down * fallForce * Time.deltaTime;
        }

        // Transition to WalkingState if horizontal input is detected
        if (rb.linearVelocityX != 0)
        {
            controller.ChangeState(new WalkingState(controller));
        }

        // Example: Transition to JumpingState if Jump button is pressed and player hasn't jumped yet
        if (jumpAction.triggered && !controller.movementEditor.hasJumped)
        {
            controller.ChangeState(new JumpingState(controller));
            controller.movementEditor.hasJumped = true;
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

        if (controller.IsCeilinged())
        {
            //Debug.Log("Player is touching a ceiling");
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Idle State");
    }
}
