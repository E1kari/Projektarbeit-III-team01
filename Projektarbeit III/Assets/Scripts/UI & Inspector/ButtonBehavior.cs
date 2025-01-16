using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{

    [SerializeField] private Object level1Scene_;
    [SerializeField] private Object level2Scene_;
    [SerializeField] private Object level3Scene_;


    public void level1()
    {
        string scenePath = AssetDatabase.GetAssetPath(level1Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void level2()
    {
        string scenePath = AssetDatabase.GetAssetPath(level2Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void level3()
    {
        string scenePath = AssetDatabase.GetAssetPath(level3Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }
}
