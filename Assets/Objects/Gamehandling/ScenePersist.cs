using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private static ScenePersist instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ ScenePersist is active and will persist.");
        }
        else
        {
            Debug.LogWarning("⚠ Duplicate ScenePersist detected. Destroying the new one.");
            Destroy(gameObject);
            return; // Prevent further execution on the duplicate
        }
    }


    public void ResetPersistance()
    {
        Destroy(gameObject);
    }
    public void KillSelf()
    {
        Destroy(gameObject);
    }
}
