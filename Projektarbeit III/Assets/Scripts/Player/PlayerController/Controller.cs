using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Interface.IState currentState;
    public MovementEditor movementEditor;
    private float raycastRangeDown;
    private float raycastRangeLeftRight;
    private float raycastRangeUp;
    private float wallJumpCooldownTimer;

    void Update()
    {
        currentState?.UpdateState(); // Safely call UpdateState if there's a current state
        UpdateStateName();

        // Decrease the cooldown timer
        if (wallJumpCooldownTimer > 0)
        {
            wallJumpCooldownTimer -= Time.deltaTime;
        }
    }

    public void ChangeState(Interface.IState newState)
    {
        currentState?.OnExit(); // Exit the current state if it exists
        currentState = newState;
        currentState?.OnEnter(); // Enter the new state
        UpdateStateName();
    }

    private void UpdateStateName()
    {
        movementEditor.currentStateName = currentState?.GetType().Name ?? "None";
    }

    void Start()
    {
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
            Debug.Log("Player is touching a wall and walking against it");
            ChangeState(new WallStickingState(this));
        }
    }

    public bool IsGrounded()
    {
        raycastRangeDown = movementEditor.raycastDown;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastRangeDown, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    public bool IsTouchingLeftWall()
    {
        raycastRangeLeftRight = movementEditor.raycastLeftRight;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastRangeLeftRight, LayerMask.GetMask("Ground"));
        return hitLeft.collider != null;
    }

    public bool IsTouchingRightWall()
    {
        raycastRangeLeftRight = movementEditor.raycastLeftRight;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastRangeLeftRight, LayerMask.GetMask("Ground"));
        return hitRight.collider != null;
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

    public bool IsCeilinged()
    {
        raycastRangeUp = movementEditor.raycastUp;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastRangeUp, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    public void StartWallJumpCooldown()
    {
        wallJumpCooldownTimer = movementEditor.wallJumpCooldown;
    }
}
