using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform[] checkpoints;
    private Transform currentCheckpoint;
    private int currentCheckpointIndex = -1;

    public Transform player;

    void Start()
    {
        gameObject.SetActive(MainMenu.checkpointsEnabled);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentCheckpoint != null)
        {
            TeleportToCheckpoint();
        }
    }

    public void ActivateCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex > currentCheckpointIndex)
        {
            currentCheckpointIndex = checkpointIndex;
            currentCheckpoint = checkpoints[checkpointIndex];
            Debug.Log("Checkpoint " + checkpointIndex + " activated.");
        }
    }

    private void TeleportToCheckpoint()
    {
        player.position = currentCheckpoint.position;
        Debug.Log("Teleported to checkpoint: " + currentCheckpointIndex);
    }
}
