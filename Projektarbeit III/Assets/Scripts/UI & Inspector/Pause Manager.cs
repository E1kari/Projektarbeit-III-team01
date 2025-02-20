using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    GameObject player;

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
        player = GameObject.FindWithTag("Player");
        player.SetActive(false);
        SceneManager.LoadScene("menu_pause", LoadSceneMode.Additive); // Load menu without unloading game
        isPaused = true;
    }

    private void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("menu_pause"); // Unload menu
        isPaused = false;
        player.SetActive(true);
        Time.timeScale = 1f; // Resume time
    }
}
