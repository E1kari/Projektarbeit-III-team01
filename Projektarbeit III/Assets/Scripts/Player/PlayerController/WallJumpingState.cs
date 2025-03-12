using UnityEngine;
using UnityEngine.InputSystem;
using static S_AudioData;

public class WallJumpingState : Interface.IState
{
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction jumpAction;
    private float jumpForceUp;
    private float jumpForceSide;
    private float moveSpeed;
    private bool jumpingFromLeftWall;
    private float wallJumpingDirection;
    private Vector2 wallJumpingPower;
    public float isWallJumping;

    public WallJumpingState(Controller controller)
    {
        this.controller = controller;
        rb = controller.GetComponent<Rigidbody2D>();
        playerInput = controller.GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Walking"];
        jumpAction = playerInput.actions["Jumping"];
        jumpForceUp = controller.movementEditor.wallJumpForce;
        jumpForceSide = controller.movementEditor.wallJumpSideForce;
        moveSpeed = controller.movementEditor.moveSpeed;
        wallJumpingPower = new Vector2(jumpForceSide, jumpForceUp);
        isWallJumping = 0.35f;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Wall Jumping State");

        // Determine which wall the player is jumping from
        jumpingFromLeftWall = controller.IsTouchingLeftWall();

        // Apply the jump force in the opposite direction of the wall
        wallJumpingDirection = jumpingFromLeftWall ? 1 : -1;
        rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    }

    public void UpdateState()
    {
        isWallJumping -= Time.deltaTime;
        if (isWallJumping <= 0)
        {
            controller.ChangeState(new FallingState(controller));
            return;
        }
    }
    public void OnDeath()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.Player_Death);
    }

    public void OnExit()
    {
        Debug.Log("Exiting Wall Jumping State");
    }
}
