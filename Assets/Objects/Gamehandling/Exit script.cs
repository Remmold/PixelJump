using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exitscript : MonoBehaviour
{
    int nextLevel;
    [SerializeField] float levelLoadDelay = 1f;

    // Update is called once per frame
    void Update()
    {
        nextLevel = SceneManager.GetActiveScene().buildIndex+1;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        StartCoroutine(LoadNextLevel());
        
        
    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        FindAnyObjectByType<ScenePersist>().ResetPersistance();
        SceneManager.LoadScene(nextLevel);
    }
}
