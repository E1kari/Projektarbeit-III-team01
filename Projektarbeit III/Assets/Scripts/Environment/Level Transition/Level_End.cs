using UnityEngine;
using UnityEngine.SceneManagement;
using static S_AudioData;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Level_End : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad_;

    [SerializeField]
    private S_Timer timer_;
    private BoxCollider2D boxCollider2D_;
    private Logger logger = Logger.Instance;
    private bool isTriggered = false;

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
        if (other.gameObject.tag == "Player" && !isTriggered) // for some reason this gets triggered twice without isTriggered and I don't know why and I hate this. It's so much clutter for nothing :/
        {
            isTriggered = true;

            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioManager.PlayAudio(AudioIndex.Environment_LevelEnd);

            GameObject.Find("Pause Manager").GetComponent<PauseManager>().TogglePause();
            timer_.StopTimer();

            if (!string.IsNullOrEmpty(sceneToLoad_))
            {
#if !UNITY_EDITOR
                Logger.Instance.Log("Loading scene: " + sceneName, "Level_End", LogType.Log);
#endif
                SceneManager.LoadScene(sceneToLoad_, LoadSceneMode.Additive);
            }
            else
            {
#if !UNITY_EDITOR
                Logger.Instance.Log("Scene name is empty or null!", "Level_End", LogType.Error);
#endif
                Debug.LogError("Scene name is empty or null!");
            }
        }
    }

    void OnDrawGizmos()
    {
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.DrawWireCube(transform.position, new Vector2(spriteSize.x - spriteSize.x / 2, spriteSize.y - spriteSize.y / 2));
    }
}
