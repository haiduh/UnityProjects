using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform oldEnvironment; // Previous environment position
    [SerializeField] private Transform newEnvironment; // Next environment position
    [SerializeField] private CameraMovement cam;       // Reference to the camera script
    private bool inNewEnvironment;                     // Flag to track current environment

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!inNewEnvironment)
            {
                Debug.Log("Switching to new environment.");
                inNewEnvironment = true;
                cam.MovetoNewEnvironment(newEnvironment); // Transition to new environment
                Invoke(nameof(EnableFollow), 1f);         // Delay to re-enable follow 
                newEnvironment.GetComponent<Environment>().activateEnv(true);
                oldEnvironment.GetComponent<Environment>().activateEnv(false);
            }
            else
            {
                Debug.Log("Switching to old environment.");
                inNewEnvironment = false;
                cam.MovetoNewEnvironment(oldEnvironment); // Transition to old environment
                Invoke(nameof(EnableFollow), 1f);         // Delay to re-enable follow 
                oldEnvironment.GetComponent<Environment>().activateEnv(true);
                newEnvironment.GetComponent<Environment>().activateEnv(false);
            }
        }
    }

    private void EnableFollow()
    {
        cam.EnableFollowMode(); // Re-enable follow mode after the transition
    }
}
