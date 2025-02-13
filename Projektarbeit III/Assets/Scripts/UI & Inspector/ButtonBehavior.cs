using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private Object button1Scene_;
    [SerializeField] private Object button2Scene_;
    [SerializeField] private Object button3Scene_;
    [SerializeField] private Object button4Scene_;

    public void button1()
    {
        string scenePath = AssetDatabase.GetAssetPath(button1Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void button2()
    {
        string scenePath = AssetDatabase.GetAssetPath(button2Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void button3()
    {
        string scenePath = AssetDatabase.GetAssetPath(button3Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void button4()
    {
        string scenePath = AssetDatabase.GetAssetPath(button3Scene_);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
