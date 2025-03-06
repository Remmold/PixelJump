using UnityEngine;

public class Checkpoint_Handler : MonoBehaviour
{
    private Transform currentCheckpoint;

    private void Awake()
    {
        currentCheckpoint = null; // Default to no checkpoint until player registers
    }

    public void RegisterPlayer(PlayerMovement player)
    {
        if (currentCheckpoint == null)
        {
            currentCheckpoint = player.transform; // Set to player’s initial position
        }
        player.SetStartingLocation(currentCheckpoint);
    }

    public void UpdateCheckpoints(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public Transform GetStartingLocation()
    {
        return currentCheckpoint;
    }
}

