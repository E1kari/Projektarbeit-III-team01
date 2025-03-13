using UnityEngine;
using UnityEngine.InputSystem;
using static S_AudioData;

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
    }

    public void UpdateState()
    {
        if (controller.IsGrounded())
        {
            controller.movementEditor.hasJumped = false;
            controller.movementEditor.hasDashed = false;
        }

        // Transition to WalkingState if horizontal input is detected
        if (movementAction.ReadValue<Vector2>().x != 0)
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
        if (dashAction.triggered && !controller.movementEditor.hasDashed && !controller.IsGrounded())
        {
            controller.ChangeState(new DashingState(controller));
            controller.movementEditor.hasDashed = true;
        }
    }

    public void OnDeath()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Player_Death);
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Idle State");
    }
}
