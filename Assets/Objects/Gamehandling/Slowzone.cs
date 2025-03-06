using UnityEngine;

public class Slowzone : MonoBehaviour
{
    bool hasTriggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered){return;}
        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

            if (player != null)
            {
                // Run the coroutine on the Player object
                player.StartCoroutine(player.TemporaryMovementChange(gravity: 0, duration: 3));
            }

            // Destroy this object AFTER starting the coroutine
            Destroy(gameObject);
        }
    }
}
