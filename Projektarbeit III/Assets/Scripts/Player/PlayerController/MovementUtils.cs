using UnityEngine;
using UnityEngine.InputSystem;

public static class MovementUtils
{
    public static void ApplyHorizontalMovement(Rigidbody2D rb, InputAction movementAction, float moveSpeed)
    {
        float moveInput = movementAction.ReadValue<Vector2>().x;
        GameObject player = rb.gameObject;

        if (Mathf.Abs(rb.linearVelocity.x) > moveSpeed)
        {
            // Maintain the current horizontal velocity to preserve momentum
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + moveInput * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
        }
        else
        {
            // Apply horizontal movement based on input
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (rb.linearVelocity.x > 0)
        {
            player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.linearVelocity.x < 0)
        {
            player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}