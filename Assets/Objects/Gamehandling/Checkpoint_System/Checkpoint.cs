#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = true;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isActive)
        {
            animator.SetBool("isActivated",true);
            Debug.Log($"Checkpoint reached at {transform.position}");

            FindAnyObjectByType<Checkpoint_Handler>().UpdateCheckpoints(transform);
            isActive = false; // Prevent retriggering
        }
    }
}

