using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsPaused => isPaused;

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Paused - Player movement disabled, UI enabled.");
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Resumed - Player movement enabled.");
    }

    public void LoadMainMenu()
    {
        
        FindAnyObjectByType<ScenePersist>().KillSelf();
        pauseMenuUI.SetActive(false);
        Debug.Log("🚀 Returning to Main Menu...");
        Time.timeScale = 1f; // Reset time
        FindAnyObjectByType<MusicVisualSync>().PauseMusic();
        SceneManager.LoadScene("MainMenu"); // Make sure you have a scene named "MainMenu"

    }
    public void QuitGame()
    {
        Debug.Log("🚀 Quitting Game...");

        #if UNITY_EDITOR
            // If running in Unity Editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running as a built game, quit application
            Application.Quit();
        #endif
    }
}
