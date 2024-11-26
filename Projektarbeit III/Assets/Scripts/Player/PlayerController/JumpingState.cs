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

        // Transition to IdleState when the player starts falling
        if (rb.linearVelocity.y <= 0)
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Transition to DashingState if Dash button is pressed and player hasn't dashed yet
        if (dashAction.triggered && !controller.movementEditor.hasDashed)
        {
            if (controller.IsGrounded())
            {
                Debug.LogError("Cannot dash while grounded");
            }
            else
            {   
            controller.ChangeState(new DashingState(controller));
            controller.movementEditor.hasDashed = true;
            }
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
