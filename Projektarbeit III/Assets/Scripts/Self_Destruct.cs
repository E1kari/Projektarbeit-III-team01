using UnityEngine;

public class Self_Destruct : MonoBehaviour
{

    [SerializeField]
    private float deathTimer = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deathTimer);
    }
}
