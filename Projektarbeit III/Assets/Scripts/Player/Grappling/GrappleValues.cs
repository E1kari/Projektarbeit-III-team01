using UnityEngine;

public class GrappleValues
{
    public static void InitializeGrappleValues(GrapplingHook grapplingHook, Controller controller)
    {
        grapplingHook.grappleSpeed = controller.movementEditor.grappleSpeed;
        grapplingHook.grappleCooldown = controller.movementEditor.grappleCooldown;
        grapplingHook.grappleRange = controller.movementEditor.grappleRange;
        grapplingHook.grappleLayer = controller.movementEditor.grappleLayer;
        grapplingHook.grappleSpeedBoost = controller.movementEditor.grappleSpeedBoost;
        grapplingHook.toleranceRadius = controller.movementEditor.toleranceRadius;
        grapplingHook.indicatorPuffer = controller.movementEditor.indicatorPuffer;
        grapplingHook.indicatorSize = controller.movementEditor.indicatorSize;
        grapplingHook.indicatorSegments = controller.movementEditor.indicatorSegments;
        grapplingHook.playedBoostAudio = false;
    }
}