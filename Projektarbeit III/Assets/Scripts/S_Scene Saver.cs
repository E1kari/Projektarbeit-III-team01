using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "S_SceneSaver", menuName = "Scriptable Objects/S_SceneSaver")]


public class S_SceneSaver : ScriptableObject
{
    private static string previousMenuScene;
    private static string currentMenuScene;

    private static string previousLevelScene;
    private static string currentLevelScene;

    private static bool showPreview = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
#if !UNITY_EDITOR
        Logger.Instance.Log("SceneSaver initialized", "SceneSaver", LogType.Log);
#endif
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower().Contains("menu"))
        {
            previousMenuScene = currentMenuScene;
            currentMenuScene = scene.name;

            determineShowPreview(scene.name);
            determineSelectedButton(scene);
        }
        else if (scene.name.ToLower().Contains("level"))
        {
            previousLevelScene = currentLevelScene;
            currentLevelScene = scene.name;
        }
    }

    public static void determineSelectedButton(Scene scene)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("default Button");

        foreach (GameObject button in buttons)
        {
            if (button.scene == scene)
            {
                GameObject.FindAnyObjectByType<EventSystem>()?.SetSelectedGameObject(button);
            }
        }
    }

    private static void determineShowPreview(string sceneName)
    {
        if (sceneName.ToLower().Equals("menu_selection"))
        {
            showPreview = true;
        }
        else if (sceneName.ToLower().Equals("menu_preview"))
        {
            showPreview = false;
        }
    }

    public bool GetShowPreview()
    {
        return showPreview;
    }

    public string GetPreviousMenuSceneName()
    {
        return previousMenuScene;
    }

    public string GetPreviousLevelSceneName()
    {
        return previousLevelScene;
    }

    public string GetCurrentLevelSceneName()
    {
        return currentLevelScene;
    }
}
