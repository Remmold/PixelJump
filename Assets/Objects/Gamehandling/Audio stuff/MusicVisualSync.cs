using UnityEngine;

public class MusicVisualSync : MonoBehaviour
{
    [SerializeField] private AudioSource music; // The music source
    [SerializeField] private float transitionToNightStartTime = 30f; // When to start transition to night
    [SerializeField] private float transitionToNightEndTime = 40f; // When to fully reach night
    [SerializeField] private float transitionToMorningStartTime = 70f; // When to start transition to morning
    [SerializeField] private float transitionToMorningEndTime = 80f; // When it's fully morning time
    [SerializeField] private SpriteRenderer background; // Reference to the background sprite
    [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.3f, 1f); // Dark blue night

    private Color originalColor;
    private bool transitioningToNight = false; // Flag to track if transition to night has started
    private bool transitioningToMorning = false; // Flag to track if transition to morning has started

    private void Start()
    {
        originalColor = background.color; // Store the starting color
    }

    private void Update()
    {
        float currentTime = music.time; // Get the current music time

        // Reset flags and background when the music resets
        if (currentTime < transitionToNightStartTime)
        {
            transitioningToNight = false;
            transitioningToMorning = false;
            background.color = originalColor;
        }

        // Transition to night
        if (!transitioningToNight && currentTime >= transitionToNightStartTime)
        {
            transitioningToNight = true; // Start transition to night
        }

        if (transitioningToNight && currentTime < transitionToNightEndTime)
        {
            // Lerp from originalColor to nightColor
            float t = Mathf.InverseLerp(transitionToNightStartTime, transitionToNightEndTime, currentTime);
            background.color = Color.Lerp(originalColor, nightColor, t);
        }
        else if (transitioningToNight && currentTime >= transitionToNightEndTime && currentTime < transitionToMorningStartTime)
        {
            // Keep the background at nightColor until morning transition starts
            background.color = nightColor;
        }

        // Transition to morning (lerp back to original color)
        if (!transitioningToMorning && currentTime >= transitionToMorningStartTime)
        {
            transitioningToMorning = true; // Start transition to morning
        }

        if (transitioningToMorning && currentTime < transitionToMorningEndTime)
        {
            // Lerp from nightColor back to originalColor
            float t = Mathf.InverseLerp(transitionToMorningStartTime, transitionToMorningEndTime, currentTime);
            background.color = Color.Lerp(nightColor, originalColor, t);
        }
        else if (transitioningToMorning && currentTime >= transitionToMorningEndTime)
        {
            // Ensure the background is reset to originalColor after the transition
            background.color = originalColor;
        }
    }

    public void PauseMusic()
    {
        music.Pause();
    }
}
