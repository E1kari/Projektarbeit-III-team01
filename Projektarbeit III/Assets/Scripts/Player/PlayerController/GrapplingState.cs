using UnityEngine;

public class GrapplingState : Interface.IState
{
    private Controller controller;
    private GrapplingHook grapplingHook;
    public GameObject grappleHead;
    private GrappleHead grappleHeadCode;

    public GrapplingState(Controller controller, GrapplingHook grapplingHook)
    {
        this.controller = controller;
        this.grapplingHook = grapplingHook;
    }

    public void OnEnter()
    {
        grappleHead = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerHead"));
        grappleHead.transform.SetParent(controller.gameObject.transform);
        grappleHeadCode = grappleHead.GetComponent<GrappleHead>();
        grappleHeadCode.LoadHead(grapplingHook, controller);
    }

    public void UpdateState()
    {
        // Handle grappling logic in the GrapplingHook component
        grapplingHook.HandleGrappling();

        // Clamp the player's velocity to prevent insane speeds
        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -controller.movementEditor.maxSpeed, controller.movementEditor.maxSpeed), rb.linearVelocity.y);
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        GameObject.Destroy(grappleHead);
    }
}