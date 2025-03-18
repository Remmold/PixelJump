using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunMoonCameraOrbit : MonoBehaviour
{
    [SerializeField] private float dayLength = 30f;

    [SerializeField] private Light2D sunLight;
    [SerializeField] private Light2D moonLight;

    [SerializeField] private Transform sunObject;
    [SerializeField] private Transform moonObject;

    [SerializeField] private float orbitRadius = 3f;
    [SerializeField] private float orbitOffsetY = -5f; // Moderate negative offset (adjustable)

    [SerializeField] private float maxSunIntensity = 1f;
    [SerializeField] private float maxMoonIntensity = 0.7f;
    [SerializeField] private float minIntensity = 0.1f;

    private float timeElapsed;

    private void Start()
    {
        sunObject.SetParent(transform, false);
        moonObject.SetParent(transform, false);

        sunLight.intensity = maxSunIntensity;
        moonLight.intensity = 0f;
    }

    private void LateUpdate()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= dayLength) timeElapsed -= dayLength;

        float cycleProgress = timeElapsed / dayLength;
        float angle = cycleProgress * 360+20f ; // Start at left horizon

        // Properly offset orbit center (relative to camera)
        Vector3 orbitCenter = new Vector3(0f, orbitOffsetY, 0f); 

        Vector3 sunPosition = orbitCenter + new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * angle) * orbitRadius,
            Mathf.Sin(Mathf.Deg2Rad * angle) * orbitRadius,
            5f);

        Vector3 moonPosition = orbitCenter + new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * (angle + 180f)) * orbitRadius,
            Mathf.Sin(Mathf.Deg2Rad * (angle + 180f)) * orbitRadius,
            5f);

        sunObject.localPosition = sunPosition;
        moonObject.localPosition = moonPosition;

        sunLight.transform.position = sunObject.position;
        moonLight.transform.position = moonObject.position;

        // Intensity management
        float halfCycle = dayLength / 2f;
        float t = (timeElapsed % halfCycle) / halfCycle;

        if (timeElapsed < halfCycle)
        {
            sunLight.intensity = Mathf.Lerp(minIntensity, maxSunIntensity, t);
            moonLight.intensity = Mathf.Lerp(maxMoonIntensity, minIntensity, t);
        }
        else
        {
            sunLight.intensity = Mathf.Lerp(maxSunIntensity, minIntensity, t);
            moonLight.intensity = Mathf.Lerp(minIntensity, maxMoonIntensity, t);
        }
    }

}
