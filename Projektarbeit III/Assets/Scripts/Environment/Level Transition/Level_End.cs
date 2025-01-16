using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_End : MonoBehaviour
{

    [SerializeField]
    private Object sceneToLoad_;

    [SerializeField]
    private S_Timer timer_;
    private BoxCollider2D boxCollider2D_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider2D_ = gameObject.AddComponent<BoxCollider2D>();
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        boxCollider2D_.size = new Vector2(spriteSize.x - spriteSize.x / 2, spriteSize.y - spriteSize.y / 2);
        boxCollider2D_.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer_.StopTimer();

            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad_);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            SceneManager.LoadScene(sceneName);
        }
    }

    void OnDrawGizmos()
    {
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.DrawWireCube(transform.position, new Vector2(spriteSize.x - spriteSize.x / 2, spriteSize.y - spriteSize.y / 2));
    }
}
