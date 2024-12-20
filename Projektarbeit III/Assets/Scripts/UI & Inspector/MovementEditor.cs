using UnityEditor;
using UnityEngine;

public class MovementEditor : MonoBehaviour
{
    [Header("Jumping Settings")]
    [Tooltip("The force applied when the player jumps. The higher the value, the higher the jump")]
    [Range(0.1f, 25f)] public float jumpForce = 10f;

    [Header("Dashing Settings")]
    [Tooltip("The speed at which the player dashes. The higher the value, the faster the dash")]
    [Range(0.1f, 50f)] public float dashSpeed = 20f;

    [Tooltip("The duration of the dash in seconds. The higher the value, the longer the dash")]
    [Range(0.1f, 5f)] public float dashDuration = 0.25f;

    [Header("Walking Settings")]
    [Tooltip("The speed at which the player moves. The higher the value, the faster the movement")]
    [Range(0.1f, 25f)] public float moveSpeed = 5f;

    [Header("Grappling Hook Settings\n(Changes apply after restarting the game)")]
    [Tooltip("The speed at which the player is pulled towards the grapple point. The higher the value, the faster the pull")]
    [Range(0.01f, 50f)] public float grappleSpeed = 20f;

    [Tooltip("The cooldown time between grapples. The higher the value, the longer the cooldown")]
    [Range(0.01f, 5f)] public float grappleCooldown = 0.5f;

    [Tooltip("The maximum distance the player can grapple. The higher the value, the longer the grapple")]
    [Range(0.1f, 50f)] public float grappleRange = 12f;

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