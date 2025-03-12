using UnityEngine;
using UnityEngine.InputSystem;
using static S_AudioData;
public class DashingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float dashSpeed;
    private float dashDuration;
    private float dashTimer;
    private Vector2 dashDirection;

    public DashingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        dashSpeed = controller.movementEditor.dashSpeed;
        dashDuration = controller.movementEditor.dashDuration;
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Dashing State");
        dashTimer = dashDuration;
        rb.gravityScale = 0f; // Disable gravity

        // Determine the dash direction
        dashDirection = Vector2.zero;
        Vector2 movementInput = controller.GetComponent<PlayerInput>().actions["Walking"].ReadValue<Vector2>();

        if (movementInput.x != 0)
        {
            dashDirection = new Vector2(movementInput.x, 0).normalized;
            if (dashDirection.x > 0)
            {
                controller.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (dashDirection.x < 0)
            {
                controller.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            // Check the sprite's facing direction and set the dash direction accordingly
            SpriteRenderer spriteRenderer = controller.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }

        // Apply the dash force
        rb.linearVelocity = dashDirection * dashSpeed;

        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Player_Dash);
    }

    public void UpdateState()
    {
        dashTimer -= Time.deltaTime;

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

        if (rb.linearVelocity.x == 0)
        {
            //Debug.Log("Player has stopped dashing (bumped against something)");
            controller.ChangeState(new FallingState(controller));
        }

        if (dashTimer <= 0)
        {
            if (controller.movementEditor.preserveDashMomentum)
            {
                // Preserve momentum by maintaining the horizontal velocity
                rb.linearVelocity = new Vector2(dashDirection.x * dashSpeed, rb.linearVelocity.y);
            }
            else
            {
                // Revert to normal air speed
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            controller.ChangeState(new FallingState(controller));
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Dashing State");
        rb.gravityScale = 1f; // Enable gravity
    }
}
