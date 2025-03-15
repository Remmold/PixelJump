using UnityEngine;

public class GameSettings : MonoBehaviour 
{
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    void Start()
    {
        StartAudioPanel(); // Open Audio settings by default
    }

    public void StopAllPanels()
    {
        audioPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void StartAudioPanel()
    {
        StopAllPanels();
        audioPanel.SetActive(true);
    }

    public void StartGameplayPanel()
    {
        StopAllPanels();
        gameplayPanel.SetActive(true);
    }

    public void StartGraphicsPanel()
    {
        StopAllPanels();
        graphicsPanel.SetActive(true);
    }

    public void StartControlsPanel()
    {
        StopAllPanels();
        controlsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        Debug.Log("Closing Options and returning to Pause Menu...");

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);   // Show Pause Menu
            Debug.Log("Pause Menu is now active.");
        }
        else
        {
            Debug.LogError("Pause Menu reference is missing!");
        }

        if (gameObject != null)
        {
            gameObject.SetActive(false); // Hide Options Menu
            Debug.Log("Options Menu is now hidden.");
        }
        else
        {
            Debug.LogError("Options Menu reference is missing!");
        }
    }


}
