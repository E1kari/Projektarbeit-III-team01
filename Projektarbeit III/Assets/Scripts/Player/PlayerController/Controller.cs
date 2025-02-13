using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Interface.IState currentState;
    private int lastStateIndex = 0;
    public S_MovementEditor movementEditor;
    private float wallJumpCooldownTimer;
    private Animator animator;

    private StateIndexingBecauseTheAnimatorIsMean stateIndex;
    private SpriteRenderer spriteRenderer;

    void Update()
    {
        currentState?.UpdateState(); // Safely call UpdateState if there's a current state

        // Decrease the cooldown timer
        if (wallJumpCooldownTimer > 0)
        {
            wallJumpCooldownTimer -= Time.deltaTime;
        }
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
        currentState?.OnExit(); // Exit the current state if it exists
        currentState = newState;
        currentState?.OnEnter(); // Enter the new state
        UpdateStateName();
        UpdatePlayerAnimator();
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

        ChangeState(new IdleState(this));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the jump and dash flags when the player touches the ground or a wall
        if (IsGrounded())
        {
            movementEditor.hasJumped = false;
            movementEditor.hasDashed = false;
        }

        // Transition to WallStickingState if the player is touching a wall and the cooldown has expired
        if (IsWalkingAgainstWall() && wallJumpCooldownTimer <= 0)
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

        // Check if the player is touching the left wall and pressing left input
        if (IsTouchingLeftWall() && movementInput.x < 0)
        {
            return true;
        }

        // Check if the player is touching the right wall and pressing right input
        if (IsTouchingRightWall() && movementInput.x > 0)
        {
            return true;
        }

        return false;
    }

    public void StartWallJumpCooldown()
    {
        wallJumpCooldownTimer = movementEditor.wallJumpCooldown;
    }
}
