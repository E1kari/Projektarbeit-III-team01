using UnityEngine;

public class Controller : MonoBehaviour
{
    private Interface.IState currentState;
    public MovementEditor movementEditor;

    private float raycastRange;
    


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
        // Reset the jump and dash flags when the player touches the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            movementEditor.hasJumped = false;
            movementEditor.hasDashed = false;
        }
    }

    public bool IsGrounded() 
    {
        raycastRange = movementEditor.raycastRange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, movementEditor.grappleLayer);
        return hit.collider != null;
    }
}
