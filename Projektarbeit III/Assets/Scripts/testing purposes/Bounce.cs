using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb_;
    [SerializeField] private float bounceForce_;

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
        StartCoroutine(AddForceOverTime(other.relativeVelocity));
    }


    IEnumerator AddForceOverTime(Vector2 pa_relativeVel)
    {
        Vector2 accBounceForce = new Vector2(pa_relativeVel.x * bounceForce_, pa_relativeVel.y * bounceForce_);
        while (accBounceForce.x != 0 || accBounceForce.y != 0)
        {
            if (accBounceForce.x < 0)
            {
                accBounceForce.x += bounceForce_;
                if (accBounceForce.x > 0)
                {
                    accBounceForce.x = 0.0f;
                }
            }
            else if (accBounceForce.x > 0)
            {
                accBounceForce.x -= bounceForce_;
                if (accBounceForce.x < 0)
                {
                    accBounceForce.x = 0.0f;
                }
            }

            if (accBounceForce.y < 0)
            {
                accBounceForce.y += bounceForce_;
                if (accBounceForce.y > 0)
                {
                    accBounceForce.y = 0.0f;
                }
            }
            else if (accBounceForce.y > 0)
            {
                accBounceForce.y -= bounceForce_;
                if (accBounceForce.y < 0)
                {
                    accBounceForce.y = 0.0f;
                }
            }

            rb_.AddForce(accBounceForce);
            yield return new WaitForFixedUpdate();
        }
    }
}
