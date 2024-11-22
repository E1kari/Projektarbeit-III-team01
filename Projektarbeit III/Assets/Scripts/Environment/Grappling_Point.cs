using UnityEngine;

public class Grappling_Point : MonoBehaviour
{

    [SerializeField]
    private S_Grappling_Point_Data grapplingPointData_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, grapplingPointData_.snappingRange_);

        Gizmos.color = new Color(0.5f, 0.6f, 1.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
