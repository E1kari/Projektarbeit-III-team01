using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            other.gameObject.GetComponent<Health>().takeDamage(1);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            other.gameObject.GetComponent<Health>().stopDamage();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

}
