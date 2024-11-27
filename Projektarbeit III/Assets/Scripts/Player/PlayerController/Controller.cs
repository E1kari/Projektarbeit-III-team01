using UnityEngine;

public class Controller : MonoBehaviour
{
    private Interface.IState currentState;
    public MovementEditor movementEditor;

    private float raycastRangeDown;
    private float raycastRangeLeftRight;
    private float raycastRangeUp;

    void Update()
    {
        currentState?.UpdateState(); // Safely call UpdateState if there's a current state
        UpdateStateName();
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
        if (IsGrounded() || IsWalled())
        {
            movementEditor.hasJumped = false;
            movementEditor.hasDashed = false;
        }
    }

    public bool IsGrounded()
    {
        raycastRangeDown = movementEditor.raycastDown;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastRangeDown, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    public bool IsWalled()
    {
        raycastRangeLeftRight = movementEditor.raycastLeftRight;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastRangeLeftRight, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastRangeLeftRight, LayerMask.GetMask("Ground"));
        return hitLeft.collider != null || hitRight.collider != null;
    }

    public bool IsCeilinged()
    {
        raycastRangeUp = movementEditor.raycastUp;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastRangeUp, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}
