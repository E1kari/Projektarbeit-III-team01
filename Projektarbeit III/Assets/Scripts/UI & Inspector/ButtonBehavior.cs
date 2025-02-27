using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonBehavior : MonoBehaviour
{
    private GameObject[] panels_;
    private S_SceneSaver sceneSaver_;
    private Logger logger = Logger.Instance;
    private List<GameObject> deactivatedObjects = new List<GameObject>();

    public void Start()
    {
        sceneSaver_ = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");

        SceneManager.sceneLoaded += OnSceneLoaded;

        panels_ = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject panel in panels_)
        {
            if (panel.name == "Control Panel")
            {
                panel.SetActive(true);
                #if !UNITY_EDITOR 
                Logger.Instance.Log("Control Panel loaded:", "Button", LogType.Log); 
                #endif
            }
            else panel.SetActive(false);
            #if !UNITY_EDITOR
            Logger.Instance.Log("Panel loaded: " + panel.name, "Button", LogType.Log);
            #endif
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            StartCoroutine(DeactivateDuplicatesCoroutine());
        }
    }

    public void LoadScene(string sceneNameToLoad)
    {
        Time.timeScale = 1f; // Resume time

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
            ReactivateDeactivatedObjects();
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);
        }
        else if (sceneNameToLoad.ToLower().Contains("menu"))
        {
            ReactivateDeactivatedObjects();
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

    private IEnumerator DeactivateDuplicatesCoroutine()
    {
        yield return null; // Ensure all components are initialized

        deactivatedObjects.Clear(); // Reset the list each time

        // Find all EventSystems in the scene
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                deactivatedObjects.Add(eventSystems[i].gameObject);
                eventSystems[i].gameObject.SetActive(false); // Deactivate but store reference
            }
        }

        // Find all AudioListeners in the scene
        AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                deactivatedObjects.Add(audioListeners[i].gameObject);
                audioListeners[i].gameObject.SetActive(false);
            }
        }
    }

    public void ReactivateDeactivatedObjects()
    {
        foreach (GameObject obj in deactivatedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        deactivatedObjects.Clear(); // Clear the list after reactivating
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
        ReactivateDeactivatedObjects();
        SceneManager.LoadScene(sceneSaver_.GetCurrentLevelSceneName());
        SceneManager.UnloadSceneAsync("menu_pause"); // Unload menu
    }

    public void resumeLevel()
    {
        GameObject.Find("Pause Manager").GetComponent<PauseManager>().TogglePause();
    }

    public void back()
    {
        ReactivateDeactivatedObjects();
        SceneManager.LoadScene(sceneSaver_.GetPreviousMenuSceneName());
    }

    public void exitGame()
    {
        Application.Quit();
    }
}