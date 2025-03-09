using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleInputHandler : MonoBehaviour
{
    public bool isUsingController;           // Whether the player is using a controller
    private Controller controller;           // Reference to the controller
    private PlayerInput playerInput;         // Reference to the PlayerInput component
    private float grappleRange;              // Maximum distance the grappling hook can reach
    private bool isCooldown;                 // Whether the grappling hook is on cooldown
    private float cooldownTimer;             // Timer to track cooldown
    public InputAction grappleAction;       // Grappling input action
    public InputAction aimAction;           // Aim input action for controller
    private Vector2 lastControllerDirection; // Last direction from the controller
    private GrapplingHook grapplingHook;     // Reference to the GrapplingHook script

    public void Initialize(GrapplingHook grapplingHook)
    {
        this.grapplingHook = grapplingHook;
        controller = GetComponent<Controller>();
        playerInput = controller.GetComponentInParent<PlayerInput>();
        grappleAction = playerInput.actions["Grappling"];
        aimAction = playerInput.actions["Aiming"];
        isCooldown = grapplingHook.isCooldown;
        cooldownTimer = grapplingHook.cooldownTimer;
        grappleRange = grapplingHook.grappleRange;
        lastControllerDirection = grapplingHook.lastControllerDirection;

        aimAction.Enable();
        grappleAction.Enable();
    }

    public void CheckPlayerInput()
    {
        // Check if the player is using a controller (for Aiming)
        isUsingController = playerInput.currentControlScheme == "Gamepad";

        if (grappleAction.triggered)
        {
            //Debug.Log("Grapple action triggered");
            grapplingHook.UpdateGrappleIndicator();
            Vector2 target;

            if (isUsingController)
            {
                // Get the aim direction from the controller
                Vector2 aimDirection = aimAction.ReadValue<Vector2>().normalized;

                if (aimDirection == Vector2.zero)
                {
                    aimDirection = lastControllerDirection;
                }
                else
                {
                    lastControllerDirection = aimDirection;
                }

                // Calculate the target based on the aim direction
                Vector2 startPosition = transform.position;
                target = startPosition + aimDirection * grappleRange;
            }
            else
            {
                // Get the target position from the mouse
                target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            }

            // Start the grapple if a valid target is found
            if (target != Vector2.zero)
            {
                grapplingHook.StartGrapple(target);
            }
        }
        else
        {
            // Update the grapple indicator position
            grapplingHook.UpdateGrappleIndicator();
        }
    }
}
