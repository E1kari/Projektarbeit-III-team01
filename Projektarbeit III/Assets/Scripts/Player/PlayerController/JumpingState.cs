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
    private InputAction dashAction;

    public JumpingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dashing"];
        moveSpeed = controller.movementEditor.moveSpeed;
        jumpForce = controller.movementEditor.jumpForce;
        fallForce = controller.movementEditor.fallForce;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Jumping State");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply upward force
        controller.movementEditor.hasJumped = true; // Set the jump flag
    }

    public void UpdateState()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Apply gravitational pull after reaching the peak of the jump
        if (rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity += Vector2.down * fallForce * Time.deltaTime;
        }

        // Transition to IdleState when the player starts falling and is grounded
        if (rb.linearVelocity.y <= 0 && controller.IsGrounded())
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Transition to DashingState if Dash button is pressed and player hasn't dashed yet
        if (dashAction.triggered && !controller.movementEditor.hasDashed)
        {
            if (controller.IsGrounded())
            {
                Debug.Log("Player is grounded");
                Debug.Log("Cannot dash while grounded");
            }
            else
            {   
            controller.ChangeState(new DashingState(controller));
            controller.movementEditor.hasDashed = true;
            }
        }

        // Check for wall and ceiling collisions
        if (controller.IsWalkingAgainstWall())
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
        Debug.Log("Exiting Jumping State");
    }
}
