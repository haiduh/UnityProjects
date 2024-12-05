using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For referencing the Image UI component for fade effect

public class CameraMovement : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Smooth time for movement
    [SerializeField] private float positionThreshold = 0.05f; // Distance threshold to snap to target

    [Header("Follow Settings")]
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Vector2 followOffset = new Vector2(2f, 0); // Look-ahead offset
    [SerializeField] private float cameraSpeed = 5f; // Camera speed
    [SerializeField] private bool enableFollow = true; // Toggle follow mode

    [Header("Transition Settings")]
    [SerializeField] private Image blackScreen; // Reference to the black screen UI Image
    [SerializeField] private float fadeDuration = 1f; // Fade duration for transitions

    private float targetPosX; // Target X position for transitions
    private Vector3 velocity = Vector3.zero; // Reference for SmoothDamp
    private float lookAhead; // Distance the camera looks ahead

    private bool isTransitioning = false; // Flag to check if transition is active

    private void Update()
    {
        if (enableFollow && player != null)
        {
            FollowPlayer();
        }
        else
        {
            HandleLevelTransition();
        }
    }

    private void FollowPlayer()
    {
        // Check player's facing direction to apply the correct look-ahead offset
        float directionMultiplier = player.localScale.x > 0 ? 1f : -1f; // If player is facing right, lookAhead is positive; if left, negative

        // Adjust the lookAhead based on player's facing direction
        lookAhead = Mathf.Lerp(lookAhead, followOffset.x * directionMultiplier, Time.deltaTime * cameraSpeed);

        // Set camera's position based on the player's position and look-ahead offset
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
    }

    private void HandleLevelTransition()
    {
        if (isTransitioning)
        {
            // Only perform smooth transition if not in follow mode
            Vector3 targetPosition = new Vector3(targetPosX, transform.position.y, transform.position.z);

            // Smoothly move the camera to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Check if the camera has reached the target position
            if (Mathf.Abs(transform.position.x - targetPosX) < positionThreshold)
            {
                transform.position = targetPosition;
                isTransitioning = false; // Transition complete
                enableFollow = true; // Re-enable follow mode
                Debug.Log("Transition complete, following player.");

                // Fade to clear after completing the transition
                StartCoroutine(FadeBlackScreen(0f));
            }
        }
    }

    public void MovetoNewEnvironment(Transform newEnvironment)
    {
        StartCoroutine(PerformTransition(newEnvironment));
    }

    private IEnumerator PerformTransition(Transform newEnvironment)
    {
        // Fade to black
        yield return StartCoroutine(FadeBlackScreen(1f));

        // Wait for a bit longer before starting the transition
        yield return new WaitForSeconds(1f); // Change 1f to your desired time

        // Perform camera movement
        isTransitioning = true; // Start the transition
        enableFollow = false; // Disable follow mode during the transition
        targetPosX = newEnvironment.position.x;
        Debug.Log($"Camera transitioning to X: {targetPosX}");

        // Wait for transition to complete
        while (isTransitioning)
        {
            yield return null;
        }

        // Fade back to clear (black screen will fade out)
        yield return StartCoroutine(FadeBlackScreen(0f));
    }

    private IEnumerator FadeBlackScreen(float targetAlpha)
    {
        // Ensure the black screen is visible
        blackScreen.gameObject.SetActive(true);

        Color screenColor = blackScreen.color;
        float startAlpha = screenColor.a;
        float elapsedTime = 0f;

        // Gradually increase/decrease alpha based on the targetAlpha
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            screenColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            blackScreen.color = screenColor;
            yield return null;
        }

        // Make sure the alpha is set to the exact targetAlpha value
        screenColor.a = targetAlpha;
        blackScreen.color = screenColor;

        // Optionally, disable the black screen to avoid blocking interactions
        if (targetAlpha == 0f)
        {
            blackScreen.gameObject.SetActive(false);
        }
    }
    public void EnableFollowMode()
    {
        enableFollow = true; // Re-enable follow mode
        Debug.Log("Follow mode enabled.");
    }
}
