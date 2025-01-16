using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    private float grappleSpeed;              // Speed at which the grappling hook moves
    private float grappleCooldown;           // Cooldown time between grappling hook uses
    private float grappleRange;              // Maximum distance the grappling hook can reach
    private LayerMask grappleLayer;          // Layer to check for grapple points
    private Vector2 grapplePoint;            // Point where the hook attaches
    private bool isGrappling;                // Whether the player is grappling
    private bool isCooldown;                 // Whether the grappling hook is on cooldown
    private Rigidbody2D rb;                  // Rigidbody for movement
    private Controller controller;           // Reference to the player's state manager
    private LineRenderer lineRenderer;       // Visual representation of the rope
    private float cooldownTimer;             // Timer to track cooldown
    private LineRenderer grappleIndicator;   // Visual indicator for the potential grapple point
    private PlayerInput playerInput;         // Player input reference
    private InputAction grappleAction;       // Grappling input action
    private InputAction rightClickAction;    // Right click input action

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller>();
        lineRenderer = GetComponent<LineRenderer>();
        playerInput = controller.GetComponent<PlayerInput>();
        grappleAction = playerInput.actions["Grappling"];

        // Initialize the LineRenderer to hide the rope
        lineRenderer.positionCount = 0;

        // Set the width of the LineRenderer
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        // Get values from MovementEditor
        grappleSpeed = controller.movementEditor.grappleSpeed;
        grappleCooldown = controller.movementEditor.grappleCooldown;
        grappleRange = controller.movementEditor.grappleRange;
        grappleLayer = controller.movementEditor.grappleLayer;

        // Create the grapple indicator
        grappleIndicator = new GameObject("GrappleIndicator").AddComponent<LineRenderer>();
        grappleIndicator.startWidth = 0.1f;
        grappleIndicator.endWidth = 0.1f;
        grappleIndicator.positionCount = 0;
        grappleIndicator.material = new Material(Shader.Find("Sprites/Default"));
        grappleIndicator.startColor = Color.blue;
        grappleIndicator.endColor = Color.blue;

        // Register input callbacks
        grappleAction.started += ctx => StartGrapple(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        grappleAction.canceled += ctx => StopGrapple();
        grappleAction.Enable();

        // Bind right mouse button as a separate action
        rightClickAction = new InputAction(binding: "<Mouse>/rightButton");
        rightClickAction.performed += ctx => Debug.Log("Right Mouse Clicked");
        rightClickAction.performed += ctx => StartGrapple(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        rightClickAction.canceled -= ctx => StopGrapple();
        rightClickAction.Enable();
    }

    private void OnDestroy()
    {
        // Unregister input callbacks
        grappleAction.performed -= ctx => Debug.Log("Grapple Button Clicked");
        grappleAction.started -= ctx => StartGrapple(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        grappleAction.canceled -= ctx => StopGrapple();
        grappleAction.Disable();
        
        rightClickAction.performed -= ctx => Debug.Log("Right Mouse Clicked");
        rightClickAction.started -= ctx => StartGrapple(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        rightClickAction.canceled -= ctx => StopGrapple();
        rightClickAction.Disable();
    }

    void Update()
    {
        // Handle cooldown timer
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
            }
        }

        // Update the grapple indicator position
        UpdateGrappleIndicator();

        if (grappleAction.triggered)
        {
            Debug.Log("Grapple action triggered");
        }
        if (rightClickAction.triggered)
        {
            Debug.Log("Right click action triggered");
        }
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            HandleGrappling();
        }
    }

    public void HandleGrappling()
    {
        // Move the player towards the grapple point
        Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * grappleSpeed;

        if (!rightClickAction.IsPressed() && !grappleAction.IsPressed()) // Check if the player cancels the grapple
        {
            StopGrapple();
        }

        // Stop grappling if the player reaches the grapple point
        if (Vector2.Distance(transform.position, grapplePoint) < 0.5f)
        {
            StopGrapple();
        }

        // Check for wall and ceiling collisions
        if (controller.IsTouchingLeftWall() || controller.IsTouchingRightWall() || controller.IsCeilinged())
        {
            StopGrapple();
        }

        // Update the rope's visual position
        UpdateLineRenderer();
    }

    private void StartGrapple(Vector2 target)
    {
        if (isCooldown && !isGrappling) // Check if the hook is on cooldown and not already grappling
        {
            Debug.Log("Hook is on cooldown and/or already grappling");
            return;
        }

        // Check if the target is within range
        if (Vector2.Distance(transform.position, target) > grappleRange)
        {
            Debug.Log("Target out of range");
            return;
        }

        // Cast a ray towards the target to find a valid grapple point
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grappleRange, grappleLayer);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;

            // Switch to the GRAPPLING state
            controller.ChangeState(new GrapplingState(controller, this));

            // Enable the rope and update its positions
            lineRenderer.positionCount = 2;
            UpdateLineRenderer();
        }
    }

    private void StopGrapple()
    {
        // Stop grappling, Start the cooldown timer and hide the rope
        isGrappling = false; 
        isCooldown = true;
        cooldownTimer = grappleCooldown;        
        lineRenderer.positionCount = 0;

        if (controller.IsWalkingAgainstWall())
        {
            Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(new WallStickingState(controller));
        }
        else if (controller.IsCeilinged())
        {
            Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new IdleState(controller));
        }
        else
        {
            Debug.Log("Stopped grappling with no collision");
            controller.ChangeState(new IdleState(controller));            
        }
    }

    private void UpdateLineRenderer()
    {
        // Draw the rope between the player and the grapple point
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, transform.position); // Start of the rope (player position)
            lineRenderer.SetPosition(1, grapplePoint);       // End of the rope (grapple point)
        }
    }

    private void UpdateGrappleIndicator()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grappleRange, grappleLayer);

        if (hit.collider != null && Vector2.Distance(transform.position, hit.point) <= grappleRange)
        {
            // Update the grapple indicator position
            DrawCircle(grappleIndicator, hit.point, 0.5f, 20);
            grappleIndicator.enabled = true;
        }
        else
        {
            grappleIndicator.enabled = false;
        }
    }

    private void DrawCircle(LineRenderer lineRenderer, Vector2 position, float radius, int segments)
    {
        lineRenderer.positionCount = segments + 1; 
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }
}
