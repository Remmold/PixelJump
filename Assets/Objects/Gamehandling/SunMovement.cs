using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class SunMovement : MonoBehaviour
{
    [SerializeField] private float dayLength = 30f; // Full cycle duration in seconds
    [SerializeField] private Light2D sunLight;
    [SerializeField] private Light2D moonLight;
    [SerializeField] private Transform cameraTransform; // Reference to camera
    [SerializeField] private float orbitRadius = 3f; // Distance from the camera

    // Light Intensities
    [SerializeField] private float maxSunIntensity = 1.0f;
    [SerializeField] private float maxMoonIntensity = 0.7f;
    [SerializeField] private float minIntensity = 0.1f; // Prevents lights from turning off completely

    private float timeElapsed = 0f;

    private void Start()
    {
        sunLight.intensity = maxSunIntensity; // Start sun fully bright
        moonLight.intensity = 0f; // Moon starts completely off

        // Ensure lights are placed correctly
        sunLight.transform.position = cameraTransform.position + new Vector3(0, 5, 5);
        moonLight.transform.position = cameraTransform.position + new Vector3(0, -5, 5);
    }


    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= dayLength) timeElapsed = 0f; // Reset cycle

        float cycleProgress = timeElapsed / dayLength; // 0 to 1 across full cycle
        float angle = cycleProgress * 360f; // Full rotation

        // ☀️ Move Sun & Moon around the camera, keeping them within view
        Vector3 sunPosition = new Vector3(
        cameraTransform.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitRadius, 
        cameraTransform.position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitRadius, 
        5// ✅ Force the absolute Z-position


        );
        Vector3 moonPosition = new Vector3(
            cameraTransform.position.x - (sunPosition.x - cameraTransform.position.x), 
            cameraTransform.position.y - (sunPosition.y - cameraTransform.position.y), 
            -5 // ✅ Force the absolute Z-position
        );

        sunLight.transform.position = sunPosition;
        moonLight.transform.position = moonPosition;

        // 🌞🌙 Improved Light Transition Logic
        float t = Mathf.Clamp01((timeElapsed % (dayLength / 2)) / (dayLength / 2)); // Ensure 0-1 range

        if (timeElapsed < dayLength / 2)
        {
            // ☀️ **Sunrise → Daytime (Sun brightens), Moon fades out slower**
            sunLight.intensity = minIntensity + (maxSunIntensity - minIntensity) * t;
            moonLight.intensity = maxMoonIntensity - (maxMoonIntensity - minIntensity) * (t * 0.5f); // Moon fades slower
        }
        else
        {
            // 🌙 **Sunset → Nighttime (Sun fades slower), Moon brightens**
            sunLight.intensity = maxSunIntensity - (maxSunIntensity - minIntensity) * (t * 0.8f); // Sun fades slower
            moonLight.intensity = minIntensity + (maxMoonIntensity - minIntensity) * t;
        }
    }

}
