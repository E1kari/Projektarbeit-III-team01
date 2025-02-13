using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void loadScene(SceneAsset pa_scene)
    {
        if (pa_scene == null)
        {
            Debug.LogError("Scene is not set");
            return;
        }
        string scenePath = AssetDatabase.GetAssetPath(pa_scene);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        SceneManager.LoadScene(sceneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
