using UnityEngine;
using UnityEngine.InputSystem;
public class DashingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float dashSpeed;
    private float dashDuration;
    private float dashTimer;
    private Vector2 dashDirection;
    private InputAction stickAction;

    public DashingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        dashSpeed = controller.movementEditor.dashSpeed;
        dashDuration = controller.movementEditor.dashDuration;
        stickAction = controller.GetComponent<PlayerInput>().actions["WallSticking"];
    }

    public void OnEnter()
    {
        Debug.Log("Entered Dashing State");
        dashTimer = dashDuration;

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
            dashDirection = controller.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        // Apply the dash force
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    public void UpdateState()
    {
        dashTimer -= Time.deltaTime;

        // Check for wall and ceiling collisions
        if (controller.IsWalkingAgainstWall() && controller.wallJumpCooldownTimer <= 0 && stickAction.IsPressed())
        {
            Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(new WallStickingState(controller));
        }

        if (controller.IsCeilinged())
        {
            Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new IdleState(controller));
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
            controller.ChangeState(new IdleState(controller));
        }
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        Debug.Log("Exiting Dashing State");
    }
}
