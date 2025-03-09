using UnityEngine;
using UnityEngine.InputSystem;
using static Interface;

public class FallingState : IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction movementAction;
    private float moveSpeed;

    public FallingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.gameObject.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jumping"];
        dashAction = playerInput.actions["Dashing"];
        movementAction = playerInput.actions["Walking"];
        moveSpeed = controller.movementEditor.moveSpeed;
    }

    public void OnEnter()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.down * Time.deltaTime * controller.movementEditor.fallForce;
        }
    }

    public void UpdateState()
    {
        CheckExitConditions();
        MovementUtils.ApplyHorizontalMovement(rb, movementAction, moveSpeed, controller.movementEditor.maxSpeed);

        // Clamp the player's vertical velocity to prevent insane speeds
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -controller.movementEditor.maxSpeed, controller.movementEditor.maxSpeed), rb.linearVelocity.y);
    }

    public void CheckExitConditions()
    {
        if (controller.IsGrounded())
        {
            controller.ChangeState(new IdleState(controller));
        }

        if (jumpAction.triggered && !controller.movementEditor.hasJumped)
        {
            controller.ChangeState(new JumpingState(controller));
            controller.movementEditor.hasJumped = true;
        }

        // Transition to DashingState if Dash button is pressed and player hasn't dashed yet
        if (dashAction.triggered && !controller.movementEditor.hasDashed && !controller.IsGrounded())
        {
            controller.ChangeState(new DashingState(controller));
            controller.movementEditor.hasDashed = true;
        }

        // Check for wall and ceiling collisions
        WallStickingState wallStickingState = new WallStickingState(controller);
        if (wallStickingState.StickingCheck())
        {
            //Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(wallStickingState);
        }
    }

    public void OnExit()
    {

    }

    public void OnDeath()
    {

    }
}
