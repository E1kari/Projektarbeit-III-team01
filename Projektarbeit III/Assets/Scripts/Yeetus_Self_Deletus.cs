using UnityEngine;

public class Yeetus_Self_Deletus : MonoBehaviour
{

    [SerializeField]
    private float didADieCountdown = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, didADieCountdown);
    }
}
