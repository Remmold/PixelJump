using UnityEngine;

public class Checkpoint_Handler : MonoBehaviour
{
    private Transform currentCheckpoint;

    private void Awake()
    {
        Debug.Log("🟢 Checkpoint_Handler Awake called.");

        if (currentCheckpoint != null)
        {
            Debug.Log($"✅ Loaded checkpoint from previous session: {currentCheckpoint.position}");
        }
        else
        {
            Debug.LogWarning("❌ currentCheckpoint is NULL in Awake! It may have been reset.");
            
            // Try to restore from PlayerPrefs (temporary backup)
            if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY"))
            {
                float x = PlayerPrefs.GetFloat("CheckpointX");
                float y = PlayerPrefs.GetFloat("CheckpointY");

                GameObject checkpointObj = new GameObject("LoadedCheckpoint");
                checkpointObj.transform.position = new Vector2(x, y);
                currentCheckpoint = checkpointObj.transform;

                Debug.Log($"✅ Restored checkpoint from PlayerPrefs: {currentCheckpoint.position}");
            }
        }
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
        if (newCheckpoint != currentCheckpoint)
        {
            Debug.Log($"✅ Checkpoint updated to: {newCheckpoint.position}");
            currentCheckpoint = newCheckpoint;
        }
        else
        {
            Debug.Log("⚠ Checkpoint already set, ignoring update.");
        }
    }


    public Transform GetStartingLocation()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("❌ GetStartingLocation() called but no checkpoint is set!");
        }
        else
        {
            Debug.Log($"✅ GetStartingLocation() returning: {currentCheckpoint.position}");
        }
        return currentCheckpoint;
    }

}

