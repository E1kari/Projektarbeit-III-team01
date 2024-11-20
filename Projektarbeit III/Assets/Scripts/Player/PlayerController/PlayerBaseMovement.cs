using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    private Vector2 _moveDirection;
    public InputActionReference move;

    private PlayerController playerController;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        move.action.Enable();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();

        if (_moveDirection.x != 0 && playerController.playerState != PlayerController.PlayerState.JUMPING)
        {
            playerController.SwitchState(PlayerController.PlayerState.WALKING);
        }
    }

    private void FixedUpdate()
    {
        if (playerController.playerState == PlayerController.PlayerState.WALKING)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = _moveDirection.x * moveSpeed;
            rb.linearVelocity = velocity;
        }
        
        if (playerController.playerState == PlayerController.PlayerState.WALKING && Mathf.Abs(rb.linearVelocity.x) < 0.01f)
        {
            playerController.SwitchState(PlayerController.PlayerState.IDLING);
        }
    }

    public void HandleWalking()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = _moveDirection.x * moveSpeed;
        rb.linearVelocity = velocity;

        if (Mathf.Abs(rb.linearVelocity.x) < 0.01f)
        {
            playerController.SwitchState(PlayerController.PlayerState.IDLING);
        }
    }
}
