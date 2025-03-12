using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static S_AudioData;

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
    public GrappleIndicator grappleIndicator;   // Indicator for the grapple point
    public GrappleChecks grappleChecks;         // Checks for the grappling hook

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller>();

        // Initialize the LineRenderer (rope)
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = DrawingUtils.InitializeLineRenderer(gameObject, 0.2f, 0.2f);

        // Get values from MovementEditor
        GrappleValues.InitializeGrappleValues(this, controller);

        grappleChecks = new GrappleChecks(this, controller, rb, enemy); 
        grappleIndicator = new GrappleIndicator(this, grappleChecks, controller, grappleRange, indicatorPuffer, indicatorSize, indicatorSegments);
        grappleInputHandler = new GrappleInputHandler(this, grappleIndicator, controller, grappleRange, lastControllerDirection);  
        grappleAction = grappleInputHandler.grappleAction;
        aimAction = grappleInputHandler.aimAction;
        isUsingController = grappleInputHandler.isUsingController; 
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
        if (Vector2.Distance(transform.position, target) > grappleRange && grappleChecks.CheckIsIndicatorOnValidGrappleSpot())
        {
            //Debug.Log("Target out of range");
            return;
        }

        // Cast a ray towards the target to find a valid grapple spot
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        RaycastHit2D hit = grappleChecks.CheckFindGrapplePoint(direction, grappleRange, grappleLayer);

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
                DrawingUtils.UpdateLineRenderer(lineRenderer, transform.position, enemy.transform.position);
            }
            else
            {
                DrawingUtils.UpdateLineRenderer(lineRenderer, transform.position, grappleSpot);
            }

            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioManager.PlayAudio(AudioIndex.Player_GrapplingHook);
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
                DrawingUtils.UpdateLineRenderer(lineRenderer, transform.position, enemy.transform.position);
            }
            else
            {
                DrawingUtils.UpdateLineRenderer(lineRenderer, transform.position, grappleSpot);
            }

            // Check if the player cancels the grapple
            grappleChecks.CheckGrappleStops();
        }
    }

    public void StopGrapple()
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

        grappleChecks.CheckCollisionState(); // Check which state to transition to
    }

    private void OnDestroy()
    {
        // Unregister input callbacks
        grappleAction.Disable();
        aimAction.Disable();
    }
}
