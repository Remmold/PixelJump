using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor.UI;
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

    AudioSource audioSource;
    // Realise this shouldnt be public but just testing some stuff with audio and playermovement
    public float speedMultiplier = 0.95f;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            speedMultiplier -= 0.05f;
            audioSource.pitch = speedMultiplier;
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
            
            StartCoroutine(LoadSameLevel());

        }
        else
        {
            
        }
    }
    public void ChangeSpeedMultiplier(float amount)
    {
        speedMultiplier += amount;
        audioSource.pitch = speedMultiplier;
        
    }



    IEnumerator LoadSameLevel()
    {
        yield  return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindAnyObjectByType<PlayerMovement>().ChangeMovespeed(speedMultiplier);
    }


    IEnumerator ResetGameSession()
    {
        yield  return new WaitForSecondsRealtime(2);
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
        audioSource.pitch = speedMultiplier;
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //TIMEPOWER AREA
    public int GetTimepowerPercentage()
    {
        return Mathf.Clamp(Mathf.RoundToInt((currentTimepower / maxTimepower) * 100), 1, 100);
    }
    private void UpdateTimeSliders()
    {
        int percentage = GetTimepowerPercentage();
        
        foreach (Slider slider in timeSliders)
        {
            if (slider != null)
            {
                slider.value = percentage;
            }
        }
    }

}
