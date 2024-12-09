using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishObject : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // Ensure the object starts inactive
        gameObject.SetActive(false);

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Set the animator to idle when the object is activated
        if (animator != null)
        {
            animator.SetBool("idle", true); // Assume "idle" is a looping idle animation
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player touches the finish object
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("idle", false); // Stop idle animation
            animator.SetTrigger("disappear"); // Play the disappear animation
            StartCoroutine(DisableAfterAnimation());
            SceneManager.LoadScene(7);
        }
    }

    private System.Collections.IEnumerator DisableAfterAnimation()
    {
        // Wait for the disappear animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Disable the object
        gameObject.SetActive(false);
    }
}
