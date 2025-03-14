using UnityEngine;

public class MusicVisualSync : MonoBehaviour
{
    [SerializeField] private AudioSource music; // The music source
    [SerializeField] private float transitionStartTime = 30f; // When to start transition
    [SerializeField] private float transitionEndTime = 40f; // When to fully reach night
    [SerializeField] private SpriteRenderer background; // Reference to the background sprite
    [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.3f, 1f); // Dark blue night
    private Color originalColor;
    private bool transitioning = false; // Flag to track if transition started

    private void Start()
    {
        originalColor = background.color; // Store the starting color
    }
    private void Update()
    {
    float currentTime = music.time; // Get the current music time

    if (!transitioning && currentTime >= transitionStartTime)
    {
        transitioning = true; // Start transition
    }

    if (transitioning)
        {
            float t = Mathf.InverseLerp(transitionStartTime, transitionEndTime, currentTime);
            music.pitch = 0.95f;
        
            background.color = Color.Lerp(originalColor, nightColor, t);
        }
    }
    public void PauseMusic()
    {
        music.Pause();
        
    }

}

