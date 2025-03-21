using UnityEngine;
using UnityEngine.InputSystem;
using static S_AudioData;

public class WallStickingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction stickAction;
    private float wallJumpCooldownTimer;

    private GameObject stickingObject;

    public WallStickingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        stickAction = playerInput.actions["WallSticking"];
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Wall Sticking State");

        rb.linearVelocity = Vector2.zero; // Stop movement initially
        rb.gravityScale = 0f; // Disable gravity
        controller.movementEditor.hasJumped = false; // Reset the jump flag

        stickingObject = DetermineStickingObject();
        if (stickingObject != null && stickingObject.tag == "Falling Block")
        {
            stickingObject.GetComponent<Falling_Block>().attachObject(controller.gameObject);
        }
    }

    public void UpdateState()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>();


        // Transition to IdleState if the player is grounded or WallSticking button is released
        if (controller.IsGrounded() || !stickAction.IsPressed())
        {
            controller.ChangeState(new IdleState(controller));
            return;
        }

        // Transition to WallJumpingState if Jump button is pressed and player hasn't jumped yet
        if (jumpAction.triggered && !controller.movementEditor.hasJumped)
        {
            controller.ChangeState(new WallJumpingState(controller));
            controller.movementEditor.hasJumped = true;
            return;
        }

        // Ensure the player sticks to the wall without sliding down
        rb.linearVelocity = Vector2.zero;
    }

    public bool StickingCheck()
    {
        return controller.IsWalkingAgainstWall() && wallJumpCooldownTimer <= 0 && stickAction.IsPressed();
    }

    private GameObject DetermineStickingObject()
    {
        Vector2 direction;
        float distance = controller.movementEditor.raycastDistanceX;

        if (controller.IsTouchingLeftWall())
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }

        Vector2[] raycastOrigins = controller.GetRaycastOrigins(direction);

        foreach (var origin in raycastOrigins)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public void OnDeath()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Player_Death);
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Wall Sticking State");

        // Reset gravity scale when exiting the state
        rb.gravityScale = 1f; // Default gravity scale
        stickingObject.GetComponent<Falling_Block>()?.detachObject(controller.gameObject);
    }
}