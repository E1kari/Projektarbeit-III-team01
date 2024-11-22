using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "S_Heavy_Enemy_Data", menuName = "Scriptable Objects/S_Heavy_Enemy_Data")]
public class S_Heavy_Enemy_Data : ScriptableObject
{
    [Header("//----- Attack -----\\\\")]
    [Range(1.75f, 2.15f)]
    public float attackRange_ = 1.5f;
    [Range(1.0f, 2.0f)]
    public float attackDuration_ = 2.0f;
    [Range(1.0f, 2.0f)]
    public float attackCooldown_;
}
