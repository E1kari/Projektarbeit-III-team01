using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    private GameObject[] panels_;

    public void Start()
    {
        panels_ = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject panel in panels_)
        {
            if (panel.name == "Control Panel")
            {
                panel.SetActive(true);
            }
            else panel.SetActive(false);
        }
    }
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

    public void activatePanel(GameObject pa_panel)
    {
        if (pa_panel == null)
        {
            Debug.LogError("Panel is not set");
            return;
        }
        foreach (GameObject panel in panels_)
        {
            panel.SetActive(false);
        }
        pa_panel.gameObject.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
