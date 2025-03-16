using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer; // Reference to the Audio Mixer

    private const string VolumePrefKey = "MasterVolume"; // Key for saving volume

    void Start()
    {
        // Load saved volume or set to default (0.5 if first time)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume); 

        // Add listener to update volume when the slider moves
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // Ensure the value never reaches 0 to prevent Mathf.Log10 errors
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        
        audioMixer.SetFloat("MasterVolume", dB);

        // Save the setting
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

}
