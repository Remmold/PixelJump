using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void StartGame()
    {
        SceneManager.LoadScene("0-0");
    }
    
}
