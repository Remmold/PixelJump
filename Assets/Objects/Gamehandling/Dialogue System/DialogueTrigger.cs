using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] bool destroyOnTrigger;
    [SerializeField] private List<DialogueNode> dialogueNodes; // Handcrafted per trigger
    private bool hasTriggered = false; // Prevents multiple activations

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            FindAnyObjectByType<PlayerMovement>().StopMovement();
            hasTriggered = true; // Mark as used
            DialoguePlayer dialoguePlayer = FindAnyObjectByType<DialoguePlayer>(); // Find DialoguePlayer in the scene
            
            if (dialoguePlayer != null)
            {
                dialoguePlayer.StartDialogue(dialogueNodes); // Pass nodes and start dialogue
            }

            gameObject.SetActive(!destroyOnTrigger); // Disable trigger after use
        }
    }
}
