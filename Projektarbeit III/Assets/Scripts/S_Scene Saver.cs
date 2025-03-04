using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "S_SceneSaver", menuName = "Scriptable Objects/S_SceneSaver")]


public class S_SceneSaver : ScriptableObject
{
    private static string previousMenuScene;
    private static string currentMenuScene;
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
        // If it's a menu scene
        if (scene.name.ToLower().Contains("menu"))
        {
            previousMenuScene = currentMenuScene;

            // Update the current menu scene to the newly loaded scene
            currentMenuScene = scene.name;

            if (scene.name.ToLower().Equals("menu_selection"))
            {
                showPreview = true;
            }
            else if (scene.name.ToLower().Equals("menu_preview"))
            {
                showPreview = false;
            }

        }
        else if (scene.name.ToLower().Contains("level"))
        {
            currentLevelScene = scene.name;
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

    public string GetCurrentLevelSceneName()
    {
        return currentLevelScene;
    }
}
