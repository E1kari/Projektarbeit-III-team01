using UnityEngine;

public class GrappleChecks
{
    private GrapplingHook grapplingHook;
    private Controller controller;
    private Rigidbody2D rb;
    private Enemy enemy;

    public GrappleChecks(GrapplingHook grapplingHook, Controller controller, Rigidbody2D rb, Enemy enemy)
    {
        this.grapplingHook = grapplingHook;
        this.controller = controller;
        this.rb = rb;
        this.enemy = enemy;
    }

    public void CheckGrappleStops()
    {
        if (CheckEnemyCollision())
        {
            grapplingHook.StopGrapple();
        }

        if (!grapplingHook.grappleAction.IsPressed())
        {
            grapplingHook.StopGrapple();
        }

        if (Vector2.Distance(grapplingHook.transform.position, grapplingHook.grappleSpot) < 0.5f)
        {
            grapplingHook.StopGrapple();
        }

        if (controller.IsTouchingLeftWall() || controller.IsTouchingRightWall() || controller.IsCeilinged())
        {
            if (CheckIsMovingTowardsWall())
            {
                grapplingHook.StopGrapple();
            }
        }
    }

    public void CheckEnemyGrapple(Vector2 grapplingHookPos, Vector2 enemyPos)
    {
        Debug.Log("Distance between grappling hook and enemy: " + Vector2.Distance(grapplingHookPos, enemyPos));

        if (Vector2.Distance(grapplingHookPos, enemyPos) <= 1.7f)
        {
            grapplingHook.StopGrapple();
        }
    }

    public bool CheckIsMovingTowardsWall()
    {
        Vector2 velocity = rb.linearVelocity;
        if (controller.IsTouchingLeftWall() && velocity.x < 0)
        {
            return true;
        }
        if (controller.IsTouchingRightWall() && velocity.x > 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckEnemyCollision()
    {
        if (enemy == null)
        {
            return false;
        }

        float distance = Vector2.Distance(grapplingHook.transform.position, enemy.transform.position);
        return distance < 2f;
    }

    public void CheckCollisionState()
    {
        WallStickingState wallStickingState = new WallStickingState(controller);
        if (wallStickingState.StickingCheck())
        {
            controller.ChangeState(new WallStickingState(controller));
        }
        else if (controller.IsCeilinged())
        {
            controller.UpdatePhysicsMaterial();
            controller.ChangeState(new FallingState(controller));
        }
        else
        {
            controller.UpdatePhysicsMaterial();
            controller.ChangeState(new FallingState(controller));
        }
    }

    public bool CheckIsIndicatorOnValidGrappleSpot()
    {
        RaycastHit2D hit = Physics2D.Raycast(grapplingHook.grappleIndicator.GetIndicatorPos(), Vector2.zero, 0f, grapplingHook.grappleLayer);
        return hit.collider != null;
    }

    public RaycastHit2D CheckFindGrapplePoint(Vector2 direction, float grappleRange, LayerMask grappleLayer)
    {
        RaycastHit2D hit = Physics2D.CircleCast(grapplingHook.transform.position, grapplingHook.toleranceRadius, direction, grappleRange, grappleLayer);
        return hit;
    }
}