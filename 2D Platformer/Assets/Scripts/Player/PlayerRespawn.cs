using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private GameObject checkPointUI;

    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;
    private bool hasCheckpointBeenTouched = false; // Tracks if a checkpoint is touched


    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check for the last checkpoint
        if (currentCheckpoint == null)
        {
            playerHealth.lifeDecrease(false);
            uiManager.GameOver();

            return;
        }
  

        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();
        Camera.main.GetComponent<CameraMovement>().MovetoNewCheckpoint(currentCheckpoint.parent); //resets the camera back to the checkpoint
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; // Store the recent checkpoint
            SoundManager.instance.playSound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; // Disable the checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear");

            if (!hasCheckpointBeenTouched) // First checkpoint touched
            {
                playerHealth.ActivateLivesUI(); // Show lives UI
                hasCheckpointBeenTouched = true; // Mark checkpoint as touched
            }
        }
    }

}
