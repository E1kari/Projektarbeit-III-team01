using UnityEngine;

[CreateAssetMenu(fileName = "S_Falling_Block", menuName = "Scriptable Objects/S_Falling_Block")]
public class S_Falling_Block : ScriptableObject
{
    [Header("Falling Block")]
    [Range(0.25f, 4f)]
    public float gravityScale_ = 1f;
}