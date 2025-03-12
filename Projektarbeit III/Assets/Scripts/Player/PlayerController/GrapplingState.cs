using UnityEngine;
using static S_AudioData;

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

        // Clamp the player's velocity to prevent insane speeds
        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -controller.movementEditor.maxSpeed, controller.movementEditor.maxSpeed), rb.linearVelocity.y);
    }

    public void OnDeath()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Player_Death);
    }

    public void OnExit()
    {
        //Debug.Log("Exiting Grappling State");
    }
}