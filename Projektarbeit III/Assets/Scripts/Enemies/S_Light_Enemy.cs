
using UnityEngine;

[CreateAssetMenu(fileName = "S_Light_Enemy", menuName = "Scriptable Objects/S_Light_Enemy")]
public class S_Light_Enemy : ScriptableObject
{
    [Header("//----- General -----\\\\")]

    [Range(0.1f, 0.5f)]
    [Tooltip("The distance the Enemy has to be above ground to be considered falling.")]
    public float fallDistance_ = 0.2f;

    [Header("//----- Attack -----\\\\")]
    [Range(3.0f, 30.0f)]
    public float attackRange_ = 10.0f;
    [Range(0.1f, 3.0f)]
    public float attackCooldown_;

    [Header("//----- Projectile -----\\\\")]

    [Range(1.0f, 10.0f)]
    public float projectileSpeed_ = 1.0f;

    [Range(0.3f, 1.0f)]
    public float projectileSize_ = 0.5f;

    [HideInInspector]
    public GameObject projectilePrefab_;
}
