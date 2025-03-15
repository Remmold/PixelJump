using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenu;
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
        // If Options Menu is open, close it first instead of unpausing
        if (optionsMenu.activeSelf)
        {
            CloseOptionsMenu(); 
            return; // Stop here, don't unpause the game
        }

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
        isPaused = false;
        Debug.Log("🚀 Returning to Main Menu...");
        Time.timeScale = 1f;
        FindAnyObjectByType<MusicVisualSync>().PauseMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("🚀 Quitting Game...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenOptionsMenu()
    {
        Debug.Log("Opening Options Menu..."); // Debugging
        if (optionsMenu == null)
        {
            Debug.LogError("Options Menu is NOT assigned in the inspector!");
            return;
        }
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        optionsMenu.SetActive(true);  // Show Options Menu
    }

    public void CloseOptionsMenu()
    {
        if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false); // Hide Options Menu

            // Ensure Pause Menu is visible if game is still paused
            if (isPaused)
                pauseMenuUI.SetActive(true);
        }
    }


}
