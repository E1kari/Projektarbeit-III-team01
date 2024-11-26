using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float fallForce;
    private float moveSpeed;
    private PlayerInput playerInput;
    private InputAction dashAction;

    public WalkingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dashing"];
        moveSpeed = controller.movementEditor.moveSpeed;
        fallForce = controller.movementEditor.fallForce;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Walking State");
    }

    public void UpdateState()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Apply fall force when the player starts falling
        if (rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity += Vector2.down * fallForce * Time.deltaTime;
        }

        // Transition to IdleState if no input
        if (moveInput == 0)
        {
            controller.ChangeState(new IdleState(controller));
        }

        // Transition to JumpingState if Jump button is pressed and player hasn't jumped yet
        if (Input.GetButtonDown("Jump") && !controller.movementEditor.hasJumped)
        {
            controller.ChangeState(new JumpingState(controller));
        }

        // Transition to DashingState if Dash button is pressed and player hasn't dashed yet
        if (dashAction.triggered && !controller.movementEditor.hasDashed)
        {
            if (controller.IsGrounded())
            {
                Debug.Log("Cannot dash while grounded");
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
        Debug.Log("Exiting Walking State");
    }
}
