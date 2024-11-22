using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : Interface.IState
{
    private Controller controller;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    public IdleState(Controller controller)
    {
        this.controller = controller;
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        dashAction = playerInput.actions["Dashing"];
    }

    public void OnEnter()
    {
        Debug.Log("Entered Idle State");
        // Reset animations, stop movement, etc.
    }

    public void UpdateState()
    {
        // Example: Transition to WalkingState if horizontal input is detected
        Vector2 movementInput = movementAction.ReadValue<Vector2>();
        if (movementInput.x != 0)
        {
            controller.ChangeState(new WalkingState(controller));
        }

        // Example: Transition to JumpingState if Jump button is pressed and player hasn't jumped yet
        if (jumpAction.triggered && !controller.movementEditor.hasJumped)
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
        Debug.Log("Exiting Idle State");
    }
}
