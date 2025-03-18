using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class SunMovement : MonoBehaviour
{
    [SerializeField] private float dayLength = 30f; // Full cycle duration in seconds
    [SerializeField] private Light2D sunLight;
    [SerializeField] private Light2D moonLight;
    [SerializeField] private Transform cameraTransform; // Reference to camera
    [SerializeField] private float orbitRadius = 3f; // Distance from the camera

    // 🔆 NEW: Sun and Moon Objects
    [SerializeField] private Transform sunObject;
    [SerializeField] private Transform moonObject;

    // Light Intensities
    [SerializeField] private float maxSunIntensity = 1.0f;
    [SerializeField] private float maxMoonIntensity = 0.7f;
    [SerializeField] private float minIntensity = 0.1f; // Prevents lights from turning off completely

    private float timeElapsed = 0f;

    private void Start()
    {
        sunLight.intensity = maxSunIntensity; // Start sun fully bright
        moonLight.intensity = 0f; // Moon starts completely off

        // Ensure lights & objects are placed correctly
        sunLight.transform.position = cameraTransform.position + new Vector3(0, -100, 5);
        moonLight.transform.position = cameraTransform.position + new Vector3(0, -100, 5);
        
        sunObject.position = sunLight.transform.position;
        moonObject.position = moonLight.transform.position;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= dayLength) timeElapsed = 0f; // Reset cycle

        float cycleProgress = timeElapsed / dayLength; // 0 to 1 across full cycle
        float angle = cycleProgress * 360f - 30f; // Full rotation

        // 🔽 Lower orbit center
        float orbitOffsetY = -1.5f; 
        Vector3 orbitCenter = cameraTransform.position + new Vector3(0, orbitOffsetY, 0);

        // ☀️ Move Sun & Moon smoothly
        Vector3 sunTargetPos = new Vector3(
            orbitCenter.x + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitRadius, 
            orbitCenter.y + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitRadius, 
            5
        );

        Vector3 moonTargetPos = new Vector3(
            orbitCenter.x - (sunTargetPos.x - orbitCenter.x), 
            orbitCenter.y - (sunTargetPos.y - orbitCenter.y), 
            -5
        );

        // 🏗 Apply Lerp for smooth movement (speed = 5x per second)
        float lerpSpeed = 5f;
        sunLight.transform.position = Vector3.Lerp(sunLight.transform.position, sunTargetPos, Time.deltaTime * lerpSpeed);
        moonLight.transform.position = Vector3.Lerp(moonLight.transform.position, moonTargetPos, Time.deltaTime * lerpSpeed);

        // 🌞🌙 Also move Sun & Moon objects!
        sunObject.position = Vector3.Lerp(sunObject.position, sunTargetPos, Time.deltaTime * lerpSpeed);
        moonObject.position = Vector3.Lerp(moonObject.position, moonTargetPos, Time.deltaTime * lerpSpeed);

        // 🌞🌙 Light Transition Logic (unchanged)
        float t = Mathf.Clamp01((timeElapsed % (dayLength / 2)) / (dayLength / 2));

        if (timeElapsed < dayLength / 2)
        {
            sunLight.intensity = minIntensity + (maxSunIntensity - minIntensity) * t;
            moonLight.intensity = 0; 
        }
        else
        {   
            sunLight.intensity = 0; 
            moonLight.intensity = minIntensity + (maxMoonIntensity - minIntensity) * t;
        }
    }
}
