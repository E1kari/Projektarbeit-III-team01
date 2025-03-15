using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private S_SceneSaver sceneSaver_;
    private bool isPaused = false;
    GameObject player;
    PlayerInput playerInput;
    InputAction pauseAction;

    public void Start()
    {
        StartCoroutine(OnSceneLoaded());
    }

    private IEnumerator OnSceneLoaded()
    {
        yield return null;
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];

        player = GameObject.FindWithTag("Player");

        sceneSaver_ = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");
        if (SceneManager.GetSceneByName("Training Room").isLoaded)
        {
            ResumeGame();
            yield return null;
        }
        if (sceneSaver_.GetShowPreview() && !SceneManager.GetSceneByName("Training Room").isLoaded)
        {
            previewMenu();
        }

    }

    public void Update()
    {
        if (pauseAction != null && pauseAction.WasPressedThisFrame())
        {
            TogglePause(true);
        }
    }

    public void TogglePause(bool loadPauseMenu = false)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame(loadPauseMenu);
        }
    }

    private void PauseGame(bool loadPauseMenu)
    {
        Time.timeScale = 0f; // Freeze time
        player?.SetActive(false);
        if (loadPauseMenu)
        {
            SceneManager.LoadScene("menu_pause", LoadSceneMode.Additive); // Load menu without unloading game
        }
        isPaused = true;
    }

    private void ResumeGame()
    {
        if (SceneManager.GetSceneByName("menu_pause").isLoaded)
        {
            SceneManager.UnloadSceneAsync("menu_pause"); // Unload menu
        }
        isPaused = false;
        player.SetActive(true);
        Time.timeScale = 1f; // Resume time
    }

    private void previewMenu()
    {
        PauseGame(false);
        SceneManager.LoadScene("menu_preview", LoadSceneMode.Additive);
    }
}
