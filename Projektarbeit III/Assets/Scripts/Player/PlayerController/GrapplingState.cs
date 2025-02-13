using UnityEngine;

public class GrapplingState : Interface.IState
{
    private Controller controller;
    private GrapplingHook grapplingHook;

    public GrapplingState(Controller controller, GrapplingHook grapplingHook)
    {
        this.controller = controller;
        this.grapplingHook = grapplingHook;
    }

    public void OnEnter()
    {
        //Debug.Log("Entered Grappling State");
    }

    public void UpdateState()
    {
        // Handle grappling logic in the GrapplingHook component
        grapplingHook.HandleGrappling();
    }

    public void OnDeath()
    {
        // Handle death logic
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Grappling State");
    }
}