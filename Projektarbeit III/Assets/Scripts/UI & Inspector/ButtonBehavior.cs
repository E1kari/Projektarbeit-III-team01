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

        SceneManager.sceneLoaded += OnSceneLoaded;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            StartCoroutine(DeactivateDuplicatesCoroutine());
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



        if (sceneName.ToLower().Contains("level") || sceneName.ToLower().Contains("room") || sceneName.ToLower().Contains("main"))
        {
            ReactivateDeactivatedObjects();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        else if (sceneName.ToLower().Contains("menu"))
        {
            ReactivateDeactivatedObjects();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

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
