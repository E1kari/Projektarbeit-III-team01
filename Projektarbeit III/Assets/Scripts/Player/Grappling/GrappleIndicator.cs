using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleIndicator
{
    private LineRenderer grappleIndicator;
    private Controller controller;
    private GrapplingHook grapplingHook;
    private float grappleRange;
    private float indicatorPuffer;
    private float indicatorSize;
    private int indicatorSegments;

    public GrappleIndicator(GrapplingHook grapplingHook, Controller controller, float grappleRange, float indicatorPuffer, float indicatorSize, int indicatorSegments)
    {
        this.grapplingHook = grapplingHook;
        this.controller = controller;
        this.grappleRange = grappleRange;
        this.indicatorPuffer = indicatorPuffer;
        this.indicatorSize = indicatorSize;
        this.indicatorSegments = indicatorSegments;

        grappleIndicator = new GameObject("GrappleIndicator").AddComponent<LineRenderer>();
        grappleIndicator.startWidth = 0.1f;
        grappleIndicator.endWidth = 0.1f;
        grappleIndicator.positionCount = 0;
        grappleIndicator.material = new Material(Shader.Find("Sprites/Default"));
        grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
        grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;
        grappleIndicator.sortingLayerID = SortingLayer.NameToID("Main Character");
    }

    public void UpdateGrappleIndicator(Vector2 playerPosition)
    {
        Vector2 direction;

        if (grapplingHook.isUsingController)
        {
            // Use the controller's aim input for direction
            direction = grapplingHook.aimAction.ReadValue<Vector2>().normalized;
            if (direction != Vector2.zero)
            {
                grapplingHook.lastControllerDirection = direction;
            }
            else
            {
                direction = grapplingHook.lastControllerDirection;
            }
        }
        else
        {
            // Use the mouse position for direction
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (mousePosition - (Vector2)playerPosition).normalized;
        }

        RaycastHit2D hit = grapplingHook.FindGrapplePoint(direction, grappleRange, grapplingHook.grappleLayer);

        if (hit.collider != null)
        {
            float distanceToHit = Vector2.Distance(playerPosition, hit.point);
            if (distanceToHit <= grappleRange + indicatorPuffer)
            {
                grappleIndicator.startColor = controller.movementEditor.indicatorColor;
                grappleIndicator.endColor = controller.movementEditor.indicatorColor;

                if (hit.collider.tag == "Light Enemy" || hit.collider.tag == "GrapplePoint")
                {
                    // Draw the grapple indicator at the hit point
                    DrawingUtils.DrawCircle(grappleIndicator, hit.transform.position, indicatorSize, indicatorSegments);
                }
                else
                {
                    // Draw the grapple indicator at the hit point
                    DrawingUtils.DrawCircle(grappleIndicator, hit.point, indicatorSize, indicatorSegments);
                }
            }
            else
            {
                grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
                grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;

                // Draw the grapple indicator at the maximum range in the direction
                Vector2 maxRangePoint = (Vector2)playerPosition + direction * grappleRange;
                DrawingUtils.DrawCircle(grappleIndicator, maxRangePoint, indicatorSize, indicatorSegments);
            }
        }
        else
        {
            grappleIndicator.startColor = controller.movementEditor.indicatorColorNotInRange;
            grappleIndicator.endColor = controller.movementEditor.indicatorColorNotInRange;

            // Draw the grapple indicator at the maximum range in the direction
            Vector2 maxRangePoint = (Vector2)playerPosition + direction * grappleRange;
            DrawingUtils.DrawCircle(grappleIndicator, maxRangePoint, indicatorSize, indicatorSegments);
        }

        if (grappleIndicator.enabled == false) grappleIndicator.enabled = true;
    }

    public Vector2 GetIndicatorPos()
    {
        return grappleIndicator.transform.position;
    }
}