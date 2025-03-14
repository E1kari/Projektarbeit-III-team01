using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleInputHandler
{
    public bool isUsingController;           // Whether the player is using a controller
    private PlayerInput playerInput;         // Reference to the PlayerInput component
    private float grappleRange;              // Maximum distance the grappling hook can reach
    public InputAction grappleAction;       // Grappling input action
    public InputAction aimAction;           // Aim input action for controller
    private Vector2 lastControllerDirection; // Last direction from the controller
    private GrapplingHook grapplingHook;     // Reference to the GrapplingHook script
    private GrappleIndicator grappleIndicator; // Reference to the GrappleIndicator script

    public GrappleInputHandler(GrapplingHook grapplingHook, GrappleIndicator grappleIndicator, Controller controller, float grappleRange, Vector2 lastControllerDirection)
    {
        this.grapplingHook = grapplingHook;
        this.grappleIndicator = grappleIndicator;
        this.grappleRange = grappleRange;
        this.lastControllerDirection = lastControllerDirection;

        playerInput = controller.GetComponentInParent<PlayerInput>();
        LoadActions();
    }

    public void CheckPlayerInput()
    {
        if (aimAction == null && grappleAction == null)
        {
            LoadActions();
        }

        CheckPlayerInputMethode();

        // Getting PlayerPos
        Vector2 playerPos = grapplingHook.transform.position;

        if (grappleAction.triggered)
        {
            //Debug.Log("Grapple action triggered");
            grappleIndicator.UpdateGrappleIndicator(playerPos);
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
                Vector2 startPosition = grapplingHook.transform.position;
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
            grappleIndicator.UpdateGrappleIndicator(playerPos);
        }
    }

    public void CheckPlayerInputMethode()
    {
        // Check if the player is using a controller
        if (playerInput.currentControlScheme == "Gamepad")
        {
            isUsingController = true;
        }
        else
        {
            isUsingController = false;
        }
    }

    public void LoadActions()
    {
        grappleAction = playerInput.actions["Grappling"];
        aimAction = playerInput.actions["Aiming"];
    }

    public void UnloadActions()
    {
        grappleAction.Disable();
        aimAction.Disable();
    }
}
