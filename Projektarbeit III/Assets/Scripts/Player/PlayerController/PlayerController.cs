using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        IDLING,
        WALKING,
        SLIDING,
        JUMPING,
        STICKING,
        DASHING,
        GRAPPLING
    }

    public PlayerState playerState = PlayerState.IDLING;
    public bool falling = false;
    public float verticalMomentum = 0f;
    public float horizontalMomentum = 0f;

    private PlayerBaseMovement playerBaseMovement;
    private PlayerJump playerJump;
    private GrapplingHook grapplingHook;

    private void Start()
    {
        playerBaseMovement = GetComponent<PlayerBaseMovement>();
        playerJump = GetComponent<PlayerJump>();
        grapplingHook = GetComponent<GrapplingHook>();
    }

    private void Update()
    {
        bool stateExecuted = ExecuteCurrentState();

        if (!stateExecuted && falling)
        {
            ExecuteFalling();
        }

        CorrectHighVelocity();
    }

    private bool ExecuteCurrentState()
    {
        switch (playerState)
        {
            case PlayerState.WALKING:
                playerBaseMovement.HandleWalking();
                return true;
            case PlayerState.JUMPING:
                playerJump.HandleJumping();
                return true;
            case PlayerState.GRAPPLING:
                grapplingHook.HandleGrappling();
                return true;
            default:
                return false;
        }
    }

    private void ExecuteFalling()
    {
        verticalMomentum -= 9.81f * Time.deltaTime;
    }

    public void SwitchState(PlayerState newState)
    {
        playerState = newState;
    }

    private void CorrectHighVelocity()
    {
        verticalMomentum = Mathf.Clamp(verticalMomentum, -20f, 20f);
        horizontalMomentum = Mathf.Clamp(horizontalMomentum, -20f, 20f);
    }
}
