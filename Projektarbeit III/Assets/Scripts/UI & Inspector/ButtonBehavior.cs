using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    private GameObject[] panels_;
    private S_SceneSaver sceneSaver_;

    private List<GameObject> deactivatedObjects = new List<GameObject>();

    public void Start()
    {
        sceneSaver_ = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");

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

        Time.timeScale = 1f; // Resume time
        if (pa_scene == null)
        {
            Debug.LogError("Scene is not set");
            return;
        }
        string scenePath = AssetDatabase.GetAssetPath(pa_scene);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);



        if (sceneName.ToLower().Contains("level") || sceneName.ToLower().Contains("room") || sceneName.ToLower().Contains("main"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        else if (sceneName.ToLower().Contains("menu"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

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

    public void resetLevel()
    {
        resumeLevel();
        SceneManager.LoadScene(sceneSaver_.GetCurrentLevelSceneName());
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public void resumeLevel()
    {
        GameObject.Find("Pause Manager").GetComponent<PauseManager>().TogglePause();
    }

    public void back()
    {
        SceneManager.UnloadSceneAsync("menu_options");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
