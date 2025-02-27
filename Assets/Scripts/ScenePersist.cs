using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {

        int numScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if(numScenePersists> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetPersistance()
    {
        Destroy(gameObject);
    }
}
