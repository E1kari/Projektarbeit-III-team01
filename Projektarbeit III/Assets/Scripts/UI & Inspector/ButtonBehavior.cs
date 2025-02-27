using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonBehavior : MonoBehaviour
{
    private GameObject[] panels_;
    private S_SceneSaver sceneSaver_;
    private Logger logger = Logger.Instance;

    public void Start()
    {
        sceneSaver_ = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");

        panels_ = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject panel in panels_)
        {
            panel.SetActive(false);
        }
    }

    private void loadScene(string pa_sceneName)
    {
        Time.timeScale = 1f; // Resume time

        if (pa_sceneName.ToLower().Contains("level") || pa_sceneName.ToLower().Contains("room") || pa_sceneName.ToLower().Contains("main")) // can be removed, when scenes are cleaned up and sorted in the build menu
        {
            SceneManager.LoadScene(pa_sceneName, LoadSceneMode.Single);

        }

        else if (pa_sceneName.ToLower().Contains("menu"))
        {
            SceneManager.LoadScene(pa_sceneName, LoadSceneMode.Additive);
        }
    }

    public void LoadScene(string sceneNameToLoad)
    {
        if (string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.LogError("Scene name is not set");
            #if !UNITY_EDITOR
            Logger.Instance.Log("Scene name is not set", "Button", LogType.Error);
            #endif
            return;
        }
        #if !UNITY_EDITOR
        Logger.Instance.Log("Scene to load: " + sceneNameToLoad, "Button", LogType.Log);
        #endif
        LoadSceneByName(sceneNameToLoad);
    }

    private void LoadSceneByName(string sceneNameToLoad)
    {
        if (sceneNameToLoad.ToLower().Contains("level") || sceneNameToLoad.ToLower().Contains("room") || sceneNameToLoad.ToLower().Contains("main"))
        {
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);
        }
        else if (sceneNameToLoad.ToLower().Contains("menu"))
        {
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Additive);
        }

        #if !UNITY_EDITOR
        Logger.Instance.Log("Scene loaded: " + sceneNameToLoad, "Button", LogType.Log);
        #endif
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    // Wrapper method to be called by the button in the Unity Inspector
    public void LoadSceneWrapper(string sceneName)
    {
        #if UNITY_EDITOR
        if (!string.IsNullOrEmpty(sceneName))
        {
            LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Invalid scene asset");
        }
        #else
        Logger.Instance.Log("Scene to load: " + sceneName, "Button", LogType.Log);
        if (!string.IsNullOrEmpty(sceneName))
        {
            Logger.Instance.Log("Scene to load: " + sceneName, "Button", LogType.Log);
            LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Invalid scene name");
            Logger.Instance.Log("Invalid scene name " + sceneName, "Button", LogType.Error);
        }
        #endif
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

    public void startLevel()
    {
        GameObject.Find("Preview Manager").GetComponent<PreviewManager>().reactivatePauseManager();
        SceneManager.UnloadSceneAsync("menu_preview");
        resumeLevel();
    }

    public void nextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        string levelName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextLevelIndex));
        SceneManager.LoadScene(levelName);
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
        Debug.Log("closing game...");
        Application.Quit();
    }
}