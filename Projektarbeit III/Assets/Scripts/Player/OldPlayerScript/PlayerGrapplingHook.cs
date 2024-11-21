using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerGrapplingHook : MonoBehaviour
{
    public float grappleSpeed = 20f;         // Speed at which the player is pulled
    public LayerMask grappleLayer;          // Layers to which the grappling hook can attach
    private Vector2 grapplePoint;           // Point where the hook attaches
    private bool isGrappling;               // Whether the player is grappling
    private Rigidbody2D rb;                 // Rigidbody for movement
    private PlayerController playerController; // Reference to the player's state manager
    private LineRenderer lineRenderer;      // Visual representation of the rope

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        lineRenderer = GetComponent<LineRenderer>();

        // Initialize the LineRenderer to hide the rope
        lineRenderer.positionCount = 0;

        // Set the width of the LineRenderer
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    void Update()
    {
        // Handle input to start grappling
        if (Input.GetMouseButtonDown(1) && !isGrappling)
        {
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
        // Cast a ray towards the target to find a valid grapple point
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, grappleLayer);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;

            // Switch to the GRAPPLING state
            playerController.SwitchState(PlayerController.PlayerState.GRAPPLING);

            // Enable the rope and update its positions
            lineRenderer.positionCount = 2;
            UpdateLineRenderer();
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;

        // Switch back to IDLING state
        playerController.SwitchState(PlayerController.PlayerState.IDLING);

        // Disable the rope visual
        lineRenderer.positionCount = 0;
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
