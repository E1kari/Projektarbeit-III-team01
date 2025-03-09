using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float fallForce;
    private float moveSpeed;
    private InputAction movementAction;
    private PlayerInput playerInput;
    private InputAction dashAction;
    private InputAction jumpAction;
    private InputAction stickAction;

    public WalkingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        dashAction = playerInput.actions["Dashing"];
        jumpAction = playerInput.actions["Jumping"];
        stickAction = playerInput.actions["WallSticking"];
        moveSpeed = controller.movementEditor.moveSpeed;
        fallForce = controller.movementEditor.fallForce;
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Walking State");
    }

    public void UpdateState()
    {
        MovementUtils.ApplyHorizontalMovement(rb, movementAction, moveSpeed, controller.movementEditor.maxSpeed);

        // Apply fall force when the player starts falling
        if (!controller.IsGrounded())
        {
            controller.ChangeState(new FallingState(controller));
        }

        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -controller.movementEditor.maxSpeed, controller.movementEditor.maxSpeed), rb.linearVelocity.y);

        // Transition to IdleState if no input
        if (movementAction.ReadValue<Vector2>().x == 0)
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Transition to JumpingState if Jump button is pressed and player hasn't jumped yet
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

        // Check for wall and ceiling collisions
        WallStickingState wallStickingState = new WallStickingState(controller);
        if (wallStickingState.StickingCheck())
        {
            //Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(new WallStickingState(controller));
        }

        if (controller.IsCeilinged())
        {
            //Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new FallingState(controller));
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Walking State");
    }
}
