using UnityEngine;

[CreateAssetMenu(fileName = "S_Breakable_Ground_Data", menuName = "Scriptable Objects/S_Breakable_Ground_Data")]
public class S_Breakable_Ground_Data : ScriptableObject
{
    [Header("Breakable Ground")]

    [Range(0.0f, 5f)]
    public float breakDelay_ = 0.5f;

    [Range(-0.3f, 0.0f)]
    [Tooltip("This will be subtracted from each side of the sprite size to determine when the breaking triggers")]
    public float graceArea_ = -0.2f;
}
