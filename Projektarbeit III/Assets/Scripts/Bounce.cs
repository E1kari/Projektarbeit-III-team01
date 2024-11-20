using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bounceForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("adding force");
        StartCoroutine(AddForceOverTime(other.relativeVelocity));
    }


    IEnumerator AddForceOverTime(Vector2 relativeVel)
    {
        Vector2 accBounceForce = new Vector2(relativeVel.x * bounceForce, relativeVel.y * bounceForce);
        while (accBounceForce.x != 0 || accBounceForce.y != 0)
        {
            if (accBounceForce.x < 0)
            {
                accBounceForce.x += bounceForce;
                if (accBounceForce.x > 0)
                {
                    accBounceForce.x = 0.0f;
                }
            }
            else if (accBounceForce.x > 0)
            {
                accBounceForce.x -= bounceForce;
                if (accBounceForce.x < 0)
                {
                    accBounceForce.x = 0.0f;
                }
            }

            if (accBounceForce.y < 0)
            {
                accBounceForce.y += bounceForce;
                if (accBounceForce.y > 0)
                {
                    accBounceForce.y = 0.0f;
                }
            }
            else if (accBounceForce.y > 0)
            {
                accBounceForce.y -= bounceForce;
                if (accBounceForce.y < 0)
                {
                    accBounceForce.y = 0.0f;
                }
            }

            rb.AddForce(accBounceForce);
            yield return new WaitForFixedUpdate();
        }
    }
}
