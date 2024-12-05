using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();
        Camera.main.GetComponent<CameraMovement>().MovetoNewEnvironment(currentCheckpoint.parent); //resets the camera back to the checkpoint
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Store the recent checkpoint as the new one
            SoundManager.instance.playSound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //use collider2d instead of collider as all the colliders are in 2D
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
