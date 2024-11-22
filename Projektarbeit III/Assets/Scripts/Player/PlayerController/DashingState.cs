using UnityEngine;

public class DashingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private float dashSpeed;
    private float dashDuration;
    private float dashTimer;

    public DashingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        dashSpeed = controller.movementEditor.dashSpeed;
        dashDuration = controller.movementEditor.dashDuration;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Dashing State");
        dashTimer = dashDuration;

        // Determine the dash direction
        Vector2 dashDirection = Vector2.zero;
        if (Input.GetAxis("Horizontal") != 0)
        {
            dashDirection = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;
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
        if (dashTimer <= 0)
        {
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
