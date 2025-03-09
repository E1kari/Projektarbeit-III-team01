using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    public float grappleSpeed;              // Speed at which the grappling hook moves
    public float grappleSpeedBoost;         // Speed boost when grappling to a point
    public float grappleCooldown;           // Cooldown time between grappling hook uses
    public float grappleRange;              // Maximum distance the grappling hook can reach
    public Collider2D grappleCollider;      // Collider of the grapple point
    public LayerMask grappleLayer;          // Layer to check for grapple points
    public Vector2 grappleSpot;             // Point where the hook attaches
    public bool isGrappling;                // Whether the player is grappling
    public bool isCooldown;                 // Whether the grappling hook is on cooldown
    public Rigidbody2D rb;                  // Rigidbody for movement
    public Controller controller;           // Reference to the player's state manager
    public LineRenderer lineRenderer;       // Visual representation of the rope
    public float cooldownTimer;             // Timer to track cooldown
    public LineRenderer grappleIndicator;   // Visual indicator for the potential grapple point
    public InputAction grappleAction;       // Grappling input action
    public InputAction aimAction;           // Aim input action for controller
    public bool isUsingController;          // Whether the player is using a controller
    public Vector2 lastControllerDirection; // Last direction from the controller
    public Enemy enemy;                     // Reference to the enemy
    public float toleranceRadius;           // Tolerance for the grapple distance
    public float indicatorPuffer;           // Puffer when the hook is in the tolerance radius
    public float indicatorSize;             // Indicator size
    public int indicatorSegments;         // Amount of segments the indicator has
    public GrappleInputHandler grappleInputHandler; // Input handler for the grappling hook

    void Start()
    {
        grappleInputHandler = gameObject.AddComponent<GrappleInputHandler>();
        grappleInputHandler.Initialize(this);
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller>();
        lineRenderer = GetComponent<LineRenderer>();
        grappleAction = grappleInputHandler.grappleAction;
        aimAction = grappleInputHandler.aimAction;
        isUsingController = grappleInputHandler.isUsingController; 

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
        grappleSpeedBoost = controller.movementEditor.grappleSpeedBoost;
        toleranceRadius = controller.movementEditor.toleranceRadius;
        indicatorPuffer = controller.movementEditor.indicatorPuffer;
        indicatorSize = controller.movementEditor.indicatorSize;
        indicatorSegments = controller.movementEditor.indicatorSegments;

        // Create the grapple indicator
        grappleIndicator = new GameObject("GrappleIndicator").AddComponent<LineRenderer>();
        grappleIndicator.startWidth = 0.1f;
        grappleIndicator.endWidth = 0.1f;
        grappleIndicator.positionCount = 0;
        grappleIndicator.material = new Material(Shader.Find("Sprites/Default"));
        grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
        grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;
        grappleIndicator.sortingLayerID = SortingLayer.NameToID("Main Character");
    }

    void Update()
    {
        grappleInputHandler.CheckPlayerInput();
    }

    private void FixedUpdate()
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

        if (isGrappling)
        {
            HandleGrappling();
        }
    }

    public void StartGrapple(Vector2 target)
    {
        if (isCooldown || isGrappling) // Check if the hook is on cooldown or already grappling
        {
            //Debug.Log("Hook is on cooldown and/or already grappling." + (isCooldown ? " Cooldown: " + cooldownTimer : ""));
            return;
        }

        // Check if the target is within range
        if (Vector2.Distance(transform.position, target) > grappleRange && IsIndicatorOnValidGrappleSpot())
        {
            //Debug.Log("Target out of range");
            return;
        }

        // Cast a ray towards the target to find a valid grapple spot
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        RaycastHit2D hit = FindGrapplePoint(direction, grappleRange, grappleLayer);

        if (hit.collider != null) // Check if the ray hits a valid grapple spot
        {
            // Snap to the center of the target object if it has the specified tags
            if (hit.collider.tag == "Light Enemy" || hit.collider.tag == "GrapplePoint")
            {
                grappleSpot = hit.collider.bounds.center;
                controller.movementEditor.hasDashed = false;
                controller.movementEditor.hasJumped = false;
            }
            else
            {
                grappleSpot = hit.point;
            }

            grappleCollider = hit.collider;
            grappleCollider.tag = hit.collider.tag;
            isGrappling = true;

            // Switch to the GRAPPLING state
            controller.ChangeState(new GrapplingState(controller, this));

            // Enable the rope and update its positions
            lineRenderer.positionCount = 2;

            if (grappleCollider.tag == "Light Enemy")
            {
                enemy = grappleCollider.GetComponent<Enemy>();
                // Update the rope's visual position
                UpdateLineRenderer(transform.position, enemy.transform.position);
            }
            else
            {
                UpdateLineRenderer(transform.position, grappleSpot);
            }
        }
    }

    public void HandleGrappling()
    {
        // Move the player towards the grapple point
        Vector2 direction = (grappleSpot - (Vector2)transform.position).normalized;
        float currentGrappleSpeed = grappleSpeed;

        // Check if the grappleSpot has the tag "GrapplePoint" and apply speed boost
        if (grappleCollider != null)
        {
            if (grappleCollider.tag == "GrapplePoint")
            {
                //Debug.Log("GrapplePoint found! Applying speed boost");
                currentGrappleSpeed += grappleSpeedBoost;
            }

            if (grappleCollider.tag == "Light Enemy")
            {
                if (enemy != null)
                {
                    if (enemy.currentStateName != "EnemyGrappledState")
                    {
                        //Debug.Log("Enemy found! Changing to Grappled state");
                        enemy.ChangeState(new EnemyGrappledState(enemy));
                    }
                }
                else
                {
                    //Debug.LogError("Enemy component not found on Light Enemy");
                }
            }

            rb.linearVelocity = direction * currentGrappleSpeed;

            if (grappleCollider.tag == "Light Enemy")
            {
                // Update the rope's visual position
                UpdateLineRenderer(transform.position, enemy.transform.position);
            }
            else
            {
                UpdateLineRenderer(transform.position, grappleSpot);
            }

            // Check if the player cancels the grapple
            CheckGrappleStops();
        }
    }

    private void StopGrapple()
    {
        //Debug.Log("Stopping grapple");

        // Stop grappling, Start the cooldown timer and hide the rope
        isGrappling = false;
        isCooldown = true;
        cooldownTimer = grappleCooldown;
        lineRenderer.positionCount = 0;

        if (grappleCollider.tag == "Light Enemy" && enemy.currentStateName == "EnemyGrappledState")
        {
            enemy = grappleCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Enemy found and in GrappleState! Changing to Falling state");
                enemy.ChangeState(new EnemyFallingState(enemy));
            }
            else
            {
                //Debug.LogError("Enemy component not found on Light Enemy");
            }
        }

        CheckCollisionState(); // Check which state to transition to
    }

    private void CheckGrappleStops()
    {
        if (CheckEnemyCollision())
        {
            //Debug.Log("Player is near the enemy. Stopping grapple.");
            StopGrapple();
        }

        // Check if the player cancels the grapple by releasing the grapple button
        if (!grappleAction.IsPressed())
        {
            //Debug.Log("Grapple action released");
            StopGrapple();
        }

        // Stop grappling if the player reaches the grapple point
        if (Vector2.Distance(transform.position, grappleSpot) < 0.5f)
        {
            //Debug.Log("Player reached the grapple point");
            StopGrapple();
        }

        // Check for wall and ceiling collisions
        if (controller.IsTouchingLeftWall() || controller.IsTouchingRightWall() || controller.IsCeilinged())
        {
            if (IsMovingTowardsWall())
            {
                //Debug.Log("Player is touching a wall or ceiling and moving towards it");
                StopGrapple();
            }
        }
    }

    private bool IsMovingTowardsWall()
    {
        Vector2 velocity = rb.linearVelocity;
        if (controller.IsTouchingLeftWall() && velocity.x < 0)
        {
            return true;
        }
        if (controller.IsTouchingRightWall() && velocity.x > 0)
        {
            return true;
        }
        return false;
    }
    private bool CheckEnemyCollision()
    {
        if (enemy == null)
        {
            //Debug.LogWarning("Enemy reference is null");
            return false;
        }

        float distance = Vector2.Distance(transform.position, enemy.transform.position);
        //Debug.Log($"Distance to enemy: {distance}");

        return distance < 2f;   // Check if the player is near an enemy
    }

    private void CheckCollisionState()
    {
        // Check for wall and ceiling collisions
        WallStickingState wallStickingState = new WallStickingState(controller);
        if (wallStickingState.StickingCheck())
        {
            //Debug.Log("Player is touching a wall and walking against it");
            controller.ChangeState(new WallStickingState(controller));
        }
        else if (controller.IsCeilinged())
        {
            controller.UpdatePhysicsMaterial();
            //Debug.Log("Player is touching a ceiling");
            controller.ChangeState(new FallingState(controller));
        }
        else
        {
            controller.UpdatePhysicsMaterial();
            //Debug.Log("Stopped grappling with no collision");
            controller.ChangeState(new FallingState(controller));
        }
    }

    private bool IsIndicatorOnValidGrappleSpot()
    {
        RaycastHit2D hit = Physics2D.Raycast(grappleIndicator.transform.position, Vector2.zero, 0f, grappleLayer);
        return hit.collider != null;
    }

    public RaycastHit2D FindGrapplePoint(Vector2 direction, float grappleRange, LayerMask grappleLayer)
    {
        // Find a valid grapple point in the direction with a CircleCast
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, toleranceRadius, direction, grappleRange, grappleLayer);

        return hit;
    }

    public void UpdateLineRenderer(Vector2 playerPosition, Vector2 grapplePosition)
    {
        // Draw the rope between the player and the grapple point
        if (isGrappling)
        {
            //Debug.Log("Updating line renderer for PlayerPos and GrapplePos: " + playerPosition + " " + grapplePosition);
            lineRenderer.SetPosition(0, playerPosition);    // Start of the rope (player position)
            lineRenderer.SetPosition(1, grapplePosition);   // End of the rope (grapple point)
        }
    }

    public void UpdateGrappleIndicator()
    {
        Vector2 direction;
        Vector2 playerPosition = transform.position;

        if (isUsingController)
        {
            // Use the controller's aim input for direction
            direction = aimAction.ReadValue<Vector2>().normalized;
            if (direction != Vector2.zero)
            {
                lastControllerDirection = direction;
            }
            else
            {
                direction = lastControllerDirection;
            }
        }
        else
        {
            // Use the mouse position for direction
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (mousePosition - (Vector2)playerPosition).normalized;
        }

        RaycastHit2D hit = FindGrapplePoint(direction, grappleRange, grappleLayer);

        if (hit.collider != null)
        {
            float distanceToHit = Vector2.Distance(playerPosition, hit.point);
            if (distanceToHit <= grappleRange + indicatorPuffer)
            {
                grappleIndicator.startColor = controller.movementEditor.indicatorColor;
                grappleIndicator.endColor = controller.movementEditor.indicatorColor;

                if (hit.collider.tag == "Light Enemy" || hit.collider.tag == "GrapplePoint")
                {
                    // Draw the grapple indicator at the hit point
                    DrawingUtils.DrawCircle(grappleIndicator, hit.transform.position, indicatorSize, indicatorSegments);
                }
                else
                {
                    // Draw the grapple indicator at the hit point
                    DrawingUtils.DrawCircle(grappleIndicator, hit.point, indicatorSize, indicatorSegments);
                }
            }
            else
            {
                grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
                grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;

                // Draw the grapple indicator at the maximum range in the direction
                Vector2 maxRangePoint = (Vector2)playerPosition + direction * grappleRange;
                DrawingUtils.DrawCircle(grappleIndicator, maxRangePoint, indicatorSize, indicatorSegments);
            }
        }
        else
        {
            grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
            grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;

            // Draw the grapple indicator at the maximum range in the direction
            Vector2 maxRangePoint = (Vector2)playerPosition + direction * grappleRange;
            DrawingUtils.DrawCircle(grappleIndicator, maxRangePoint, indicatorSize, indicatorSegments);
        }

        if (grappleIndicator.enabled == false) grappleIndicator.enabled = true;
    }

    private void OnDestroy()
    {
        // Unregister input callbacks
        grappleAction.Disable();
        aimAction.Disable();
    }
}
