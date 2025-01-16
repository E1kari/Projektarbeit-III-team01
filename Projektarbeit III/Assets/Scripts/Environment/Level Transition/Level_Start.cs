using UnityEngine;

public class Level_Start : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab_;

    [SerializeField]
    private S_Timer timer_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        float playerHeight = playerPrefab_.GetComponent<SpriteRenderer>().bounds.size.y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.0f, height / 2.0f, 0.0f), Vector2.down);
        if (hit)
        {
            Instantiate(playerPrefab_, hit.point + new Vector2(0.0f, playerHeight / 2.0f), transform.rotation);
        }
        else
        {
            Debug.LogWarning("No ground for player spawning found");
            Instantiate(playerPrefab_, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
        }
        timer_.StartTimer();
    }
}
