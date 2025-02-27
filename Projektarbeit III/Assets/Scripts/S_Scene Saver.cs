using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "S_SceneSaver", menuName = "Scriptable Objects/S_SceneSaver")]
public class S_SceneSaver : ScriptableObject
{
    private static string previousMenuScene;
    private static string currentMenuScene;
    private static string currentLevelScene;
    private Logger logger = Logger.Instance;

    // This ensures the method runs when the game starts
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
        }
        else if (scene.name.ToLower().Contains("level") || scene.name.ToLower().Contains("room"))
        {
            currentLevelScene = scene.name;
        }
        #if !UNITY_EDITOR
        Logger.Instance.Log("Scene loaded: " + scene.name, "SceneSaver", LogType.Log);
        #endif
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
