using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "S_MovementEditor", menuName = "Scriptable Objects/S_MovementEditor")]
public class S_MovementEditor : ScriptableObject
{
    [Header("Jumping Settings")]
    [Tooltip("The force applied when the player jumps. The higher the value, the higher the jump")]
    [Range(0.1f, 25f)] public float jumpForce = 8f;

    [Tooltip("The force applied to the player when they fall. The higher the value, the faster the fall")]
    [Range(0.1f, 50f)] public float fallForce = 6f;

    [Header("Wall Sticking Settings")]
    [Tooltip("The duration the player can stick to a wall. The higher the value, the longer the stick duration")]
    [Range(0.1f, 10f)] public float stickDuration = 0.1f;

    [Header("Wall Jumping Settings")]
    [Tooltip("The force applied to the player when they jump off a wall upwards. The higher the value, the higher the jump")]
    [Range(0.1f, 30f)] public float wallJumpForce = 4.8f;

    [Tooltip("The force applied to the player when they jump off a wall to the side. The higher the value, the further the jump")]
    [Range(0.1f, 30f)] public float wallJumpSideForce = 6f;   

    [Tooltip("The cooldown time after a wall jump before the player can stick to a wall again. The higher the value, the longer the cooldown")]
    [Range(0.1f, 25f)] public float wallJumpCooldown = 1f;

    [Header("Raycast Settings")]
    [Tooltip("The distance the raycasts are performed in the X direction. The higher the value, the further the raycast")]
    [Range(-5f, 5f)] public float raycastDistanceX = 0.5f;

    [Tooltip("The distance the raycasts are performed in the Y direction. The higher the value, the further the raycast")]
    [Range(-5f, 5f)] public float raycastDistanceY = 1f;

    [Tooltip("Toggle to enable or disable the raycasts in the editor")]
    public bool drawRaycasts = true;

    [Header("Dashing Settings")]
    [Tooltip("The speed at which the player dashes. The higher the value, the faster the dash")]
    [Range(0.1f, 50f)] public float dashSpeed = 13.1f;

    [Tooltip("The duration of the dash in seconds. The higher the value, the longer the dash")]
    [Range(0.1f, 5f)] public float dashDuration = 0.52f;

    [Tooltip("Whether to preserve momentum after dashing. If enabled, the player will maintain their dash speed after the dash ends")]
    public bool preserveDashMomentum = true;

    [Header("Walking Settings")]
    [Tooltip("The speed at which the player moves. The higher the value, the faster the movement")]
    [Range(0.1f, 25f)] public float moveSpeed = 7.7f;

    [Header("Grappling Hook Settings")]
    [Tooltip("The speed at which the player is pulled towards the grapple point. The higher the value, the faster the pull")]
    [Range(0.01f, 50f)] public float grappleSpeed = 14.8f;

    [Tooltip("The cooldown time between grapples. The higher the value, the longer the cooldown")]
    [Range(0.01f, 5f)] public float grappleCooldown = 1f;

    [Tooltip("The maximum distance the player can grapple. The higher the value, the longer the grapple")]
    [Range(0.1f, 50f)] public float grappleRange = 13.7f;

    [Tooltip("The speedboost which the player gets by hooking onto a grapple point. The higher the value, the faster the player")]
    [Range(0.1f, 50f)] public float grappleSpeedBoost = 10f;

    [Tooltip("The tolerance for the grapple distance. The higher the value, the more forgiving the grapple/easier to hit the target")]
    [Range(-5f, 10f)] public float toleranceRadius = 0.5f;

    [Header("Indicator Settings")]
    [Tooltip("The puffer when the hook is in the tolerance radius. The higher the value, the bigger the puffer")]
    [Range(-5f, 10f)] public float indicatorPuffer = 0.55f;

    [Tooltip("The indicator size. The higher the value, the bigger the indicator")]
    [Range(-5f, 10f)] public float indicatorSize = 0.55f;

    [Tooltip("The amount of segments the indicator has. The higher the value, the more segments. Segments are basically the lines of the indicator")]
    [Range(-5f, 25f)] public int indicatorSegments = 15;

    [Header("Enemy Settings")]
    [Tooltip("The speed the player hooks the enemy with. The higher the value, the faster the pull")]
    [Range(0.01f, 50f)] public float enemyHookSpeed = 10f;

    [Tooltip("The layer to check for grapple points (e.g., walls, ground etc.)")]
    public LayerMask grappleLayer; // Layer to check for grapple points

    [Header("State Management")]
    [Tooltip("The name of the current state the player is in. Used for debugging purposes.")]
    [ReadOnly] public string currentStateName;

    [Tooltip("Flag to indicate if the player has jumped. Used for state transitions.")]
    [ReadOnly] public bool hasJumped = false;

    [Tooltip("Flag to indicate if the player has dashed. Used for state transitions.")]
    [ReadOnly] public bool hasDashed = false;
}

public class ReadOnlyAttribute : PropertyAttribute // Custom attribute to make a field read-only in the inspector
{
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) // Override the OnGUI method to make the property read-only
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label); // Draw the property field as disabled
        GUI.enabled = true;
    }
}
