using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform oldEnvironment; // Previous environment position
    [SerializeField] private Transform newEnvironment; // Next environment position
    [SerializeField] private Transform ignoreFollow; // Ignore Camera Follow 
    [SerializeField] private CameraMovement cam; // Reference to the camera script
    [SerializeField] private Transform teleportTarget; // The position where the player will teleport

    private bool inNewEnvironment; // Flag to track current environment


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!inNewEnvironment)
            {
                Debug.Log("Switching to new environment.");
                inNewEnvironment = true;

                // Stop player movement during transition
                collision.GetComponent<PlayerMovement>().canMove = false;

                // Teleport the player to the specified target position
                TeleportPlayer(collision.transform);

                cam.MovetoNewEnvironment(newEnvironment); // Transition to new environment
                Invoke(nameof(EnableFollow), 1f); // Delay to re-enable 
                newEnvironment.GetComponent<Environment>().activateEnv(true);
                oldEnvironment.GetComponent<Environment>().activateEnv(false);
            }
            else
            {
                Debug.Log("Switching to old environment.");
                inNewEnvironment = false;

                // Stop player movement during transition
                collision.GetComponent<PlayerMovement>().canMove = false;

                // Teleport the player to the specified target position
                TeleportPlayer(collision.transform);

                cam.MovetoNewEnvironment(oldEnvironment); // Transition to old environment
                Invoke(nameof(EnableFollow), 1f); // Delay to re-enable follow
                oldEnvironment.GetComponent<Environment>().activateEnv(true);
                newEnvironment.GetComponent<Environment>().activateEnv(false);
            }
        }
    }


     void TeleportPlayer(Transform player)
    {
        if (teleportTarget != null)
        {
            Debug.Log($"Teleporting player to {teleportTarget.position}");
            player.position = teleportTarget.position;
        }
        else
        {
            Debug.LogWarning("Teleport target not set!");
        }
    }

    private void EnableFollow()
    {
        if (newEnvironment == ignoreFollow)
        {
            Debug.Log("Ignoring follow mode for this environment.");
            cam.follow = false; // Disable follow mode
        }
        else
        {
            Debug.Log("Follow mode enabled for this environment.");
            cam.follow = true; // Enable follow mode
        }

        cam.FollowMode();

        // Re-enable player movement after transition is complete
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }
    }

}
