using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Interface.IState currentState;
    private Interface.IState previousState;
    private int lastStateIndex = 0;
    public S_MovementEditor movementEditor;
    private Animator animator;
    private StateIndexingBecauseTheAnimatorIsMean stateIndex;
    private SpriteRenderer spriteRenderer;
    private PhysicsMaterial2D material;
    private Rigidbody2D rb;

    void Update()
    {
        UpdatePhysicsMaterial(); // Update the physics material based on the player's velocity
        currentState?.UpdateState(); // Safely call UpdateState if there's a current state
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            //Debug.LogError("SpriteRenderer component is missing on the Player game object.");
        }
    }

    public void ChangeState(Interface.IState newState)
    {
        previousState = currentState;
        currentState?.OnExit(); // Exit the current state if it exists
        currentState = newState;
        currentState?.OnEnter(); // Enter the new state
        UpdateStateName();
        UpdatePlayerAnimator();
    }

    public Interface.IState GetPreviousState()
    {
        return previousState;
    }

    private void UpdateStateName()
    {
        movementEditor.currentStateName = currentState?.GetType().Name ?? "None";
    }

    private void UpdatePlayerAnimator()
    {
        if (animator == null)
        {
            //Debug.Log("Animator component is missing on the Player game object.");
            return;
        }
        int playerIndex = stateIndex?.GetPlayerIndex(currentState?.GetType().Name) ?? -1;

        if (playerIndex != lastStateIndex)
        {
            lastStateIndex = playerIndex;

            animator.SetInteger("State", playerIndex);
            animator.SetTrigger("switch");
        }
    }

    void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        stateIndex = Resources.Load<StateIndexingBecauseTheAnimatorIsMean>("Scriptable Objects/State indexing");
        stateIndex.init();

        movementEditor = Resources.Load<S_MovementEditor>("Scriptable Objects/S_MovementEditor");

        rb = GetComponent<Rigidbody2D>();
        material = GetComponent<Rigidbody2D>().sharedMaterial;

        ChangeState(new IdleState(this));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the jump and dash flags when the player touches the ground or a wall
        if (IsGrounded() && currentState is not GrapplingState)
        {
            movementEditor.hasJumped = false;
            movementEditor.hasDashed = false;
        }

        WallStickingState wallStickingState = new WallStickingState(this);
        // Transition to WallStickingState if the player is touching a wall, cooldown has expired and the player holds the stick button
        if (currentState is not WallStickingState && wallStickingState.StickingCheck())
        {
            //Debug.Log("Player is touching a wall and walking against it");
            ChangeState(new WallStickingState(this));
        }
    }

    private bool CheckCollision(Vector2 direction, float distance, Color debugColor)
    {
        if (spriteRenderer == null)
        {
            //Debug.LogError("SpriteRenderer component is missing on the Player game object.");
            return false;
        }

        Vector2 raycastStart = (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, direction, distance, LayerMask.GetMask("Ground"));
        if (movementEditor.drawRaycasts)
        {
            //Debug.DrawRay(raycastStart, direction * distance, debugColor, 2f);
        }
        return hit.collider != null;
    }

    public bool IsGrounded()
    {
        return CheckCollision(Vector2.down, movementEditor.raycastDistanceY, Color.red);
    }

    public bool IsTouchingLeftWall()
    {
        return CheckCollision(Vector2.left, movementEditor.raycastDistanceX, Color.green);
    }

    public bool IsTouchingRightWall()
    {
        return CheckCollision(Vector2.right, movementEditor.raycastDistanceX, Color.blue);
    }

    public bool IsCeilinged()
    {
        return CheckCollision(Vector2.up, movementEditor.raycastDistanceY, Color.yellow);
    }

    public bool IsWalkingAgainstWall()
    {
        Vector2 movementInput = GetComponent<PlayerInput>().actions["Walking"].ReadValue<Vector2>();

        // Check if the player is touching the left wall
        if (IsTouchingLeftWall())
        {
            return true;
        }

        // Check if the player is touching the right wall
        if (IsTouchingRightWall())
        {
            return true;
        }

        return false;
    }

    public void UpdatePhysicsMaterial()
    {
        if (rb.linearVelocity.y < 0 && IsGrounded())
        {
            rb.sharedMaterial = null;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.sharedMaterial = material;
        }
    }
}