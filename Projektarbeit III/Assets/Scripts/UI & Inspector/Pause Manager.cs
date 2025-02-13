using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Freeze time
        SceneManager.LoadScene("menu_pause", LoadSceneMode.Additive); // Load menu without unloading game
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.UnloadSceneAsync("menu_pause"); // Unload menu
        isPaused = false;
    }
}
