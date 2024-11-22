using UnityEngine;

[CreateAssetMenu(fileName = "S_Grappling_Point_Data", menuName = "Scriptable Objects/S_Grappling_Point_Data")]
public class S_Grappling_Point_Data : ScriptableObject
{
    [Range(0.1f, 50.0f)]
    public float gainedSpeed_ = 1.0f;

    [Range(0.1f, 5.0f)]
    public float distance_ = 1.0f;

    [Range(0.0f, 1.0f)]
    public float snappingRange_ = 1.0f;
}
