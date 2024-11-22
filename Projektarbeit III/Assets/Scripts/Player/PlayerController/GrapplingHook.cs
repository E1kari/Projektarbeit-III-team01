using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    private float grappleSpeed;
    private float grappleCooldown;
    private float grappleRange;
    private LayerMask grappleLayer;          // Layer to check for grapple points

    private Vector2 grapplePoint;            // Point where the hook attaches
    private bool isGrappling;                // Whether the player is grappling
    private bool isCooldown;                 // Whether the grappling hook is on cooldown
    private Rigidbody2D rb;                  // Rigidbody for movement
    private Controller controller;           // Reference to the player's state manager
    private LineRenderer lineRenderer;       // Visual representation of the rope
    private float cooldownTimer;             // Timer to track cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller>();
        lineRenderer = GetComponent<LineRenderer>();

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

        // Handle input to start grappling
        if (Input.GetMouseButtonDown(1) && !isGrappling)
        {
            if (isCooldown)
            {
                Debug.Log("Grappling hook is on cooldown");
                return;
            }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartGrapple(mousePosition);
        }

        // Handle input to stop grappling
        if (Input.GetMouseButtonUp(1) && isGrappling)
        {
            StopGrapple();
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

        // Stop grappling if the player reaches the grapple point
        if (Vector2.Distance(transform.position, grapplePoint) < 0.5f)
        {
            StopGrapple();
        }

        // Update the rope's visual position
        UpdateLineRenderer();
    }

    private void StartGrapple(Vector2 target)
    {
        // Check if the target is within range
        if (Vector2.Distance(transform.position, target) > grappleRange)
        {
            Debug.Log("Target out of range");
            return; // Target is out of range, do not start grappling
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
        isGrappling = false;

        // Switch back to IDLING state
        controller.ChangeState(new IdleState(controller));

        // Disable the rope visual
        lineRenderer.positionCount = 0;

        // Start cooldown
        isCooldown = true;
        cooldownTimer = grappleCooldown;
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
}