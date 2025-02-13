using UnityEngine;
using UnityEngine.InputSystem;

public static class MovementUtils
{
    public static void ApplyHorizontalMovement(Rigidbody2D rb, InputAction movementAction, float moveSpeed, float maxSpeed)
    {
        float moveInput = movementAction.ReadValue<Vector2>().x;
        GameObject player = rb.gameObject;

        if (Mathf.Abs(rb.linearVelocity.x) > moveSpeed)
        {
            // Maintain the current horizontal velocity to preserve momentum
            rb.linearVelocityX = rb.linearVelocity.x + moveInput * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Apply horizontal movement based on input
            rb.linearVelocityX = moveInput * moveSpeed;
        }

        if (rb.linearVelocityX > 0) // Flip the player sprite based on movement direction
        {
            player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.linearVelocityX < 0)
        {
            player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (rb.linearVelocity.x > maxSpeed)
        {
            rb.linearVelocityX = rb.linearVelocity.normalized.x * maxSpeed;
        }
    }
}