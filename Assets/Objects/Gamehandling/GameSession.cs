using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.UI;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives;
    [SerializeField] int playerScore;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] List<Slider> timeSliders = new(); 

    float maxTimepower =100f;
    float currentTimepower ;
    int timepowerPercentage;

    // Realise this shouldnt be public but just testing some stuff with audio and playermovement
    public float speedMultiplier = 0.95f;
    void Awake()
    {
        playerLives =5;

        
        
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if(numGameSessions> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        currentTimepower = maxTimepower;


        livesText.text = playerLives.ToString();

    }
    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        { 
            TakeLife();
            livesText.text = playerLives.ToString();
        }
        else
        {
            StartCoroutine(ResetGameSession());
        }
    }

    private void TakeLife()
    {
        if(playerLives > 1)
        {
            playerLives --;
            
            StartCoroutine(RespawnPlayer());

        }
        else
        {
            
        }
    }
    public void ChangeSpeedMultiplier(float amount)
    {
        speedMultiplier += amount;  
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2); // Wait before respawn

        // Find the player and call their Respawn() method
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("✅ Respawning player...");
            player.Respawn(); // Let PlayerMovement handle the respawn logic
        }
        else
        {
            Debug.LogError("❌ Respawn failed: No player found!");
        }
    }




    IEnumerator ResetGameSession()
    {
        yield  return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
        FindAnyObjectByType<ScenePersist>().ResetPersistance();
    }
    public void IncreaseScore(int value)
    {
        playerScore += value;
        scoreText.text = playerScore.ToString();
    }
    public void AlterTime(float amount)
    {
        speedMultiplier += amount;
        if(speedMultiplier >1.1) {speedMultiplier = 1.1f;}
    }

}
