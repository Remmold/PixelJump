using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = true;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isActive)
        {
            Debug.Log($"Checkpoint reached at {transform.position}");
            FindAnyObjectByType<Checkpoint_Handler>().UpdateCheckpoints(transform);
            isActive = false; // Prevent retriggering
        }
    }
}

