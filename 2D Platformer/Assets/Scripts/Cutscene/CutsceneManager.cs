using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text narrativeText;  // Main narrative message
    [SerializeField] private Text levelText;      // Displays "Level X"
    [SerializeField] private Image fadeImage;     // Fullscreen black Image for fade-out effect
    [SerializeField] private float typingSpeed = 0.05f; // Speed for typewriter effect
    [SerializeField] private float fadeDuration = 1f;   // Duration for fade-in and fade-out effects
    [SerializeField] private float pauseDuration = 1f;  // Pause between narrative and level text

    [Header("Text Content")]
    [SerializeField] private string narrativeMessage = "Welcome to the Game!";
    [SerializeField] private string levelMessage = "Level 1";

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;   // AudioSource component for sound effects
    [SerializeField] private AudioClip typingSound;     // Sound effect for typing

    private void Start()
    {
        // Begin the level transition sequence
        StartTransition();
    }

    private void StartTransition()
    {
        // Begin the fade-in for the narrative text
        StartCoroutine(FadeInText(narrativeText));

        // Then start the typewriter effect
        StartCoroutine(PlayTypewriterEffect());
    }

    private IEnumerator PlayTypewriterEffect()
    {
        // Clear texts and ensure levelText starts invisible
        narrativeText.text = "";
        levelText.text = levelMessage;
        levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 0); // Invisible

        // Type out the narrative message
        foreach (char letter in narrativeMessage)
        {
            narrativeText.text += letter;

            // Play typing sound
            if (SoundManager.instance != null && typingSound != null)
            {
                SoundManager.instance.playSound(typingSound); // Play typing sound at 0.15 volume
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Pause briefly, then fade out the narrative text
        yield return new WaitForSeconds(pauseDuration);
        yield return StartCoroutine(FadeOutText(narrativeText));

        // Fade in the level number
        yield return StartCoroutine(FadeInText(levelText));

        // Wait before transitioning to black
        yield return new WaitForSeconds(pauseDuration);
        StartCoroutine(FadeToBlackAndLoadNextLevel());
    }

    private IEnumerator FadeInText(Text textElement)
    {
        float elapsedTime = 0f;
        Color startColor = textElement.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully visible

        // Gradually fade in the text
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textElement.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        textElement.color = targetColor; // Ensure it's fully visible
    }

    private IEnumerator FadeOutText(Text textElement)
    {
        float elapsedTime = 0f;
        Color startColor = textElement.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fully transparent

        // Gradually fade out the text
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textElement.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        textElement.color = targetColor; // Ensure it's fully transparent
    }

    private IEnumerator FadeToBlackAndLoadNextLevel()
    {
        float elapsedTime = 0f;
        Color startColor = new Color(0, 0, 0, 0); // Transparent black
        Color targetColor = new Color(0, 0, 0, 1); // Fully opaque black

        // Gradually fade the screen to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeImage.color = targetColor; // Ensure it's fully opaque

        // Call the SoundManager to disable after the cutscene
        if (SoundManager.instance != null)
        {
            SoundManager.instance.gameObject.SetActive(false); // Disable the SoundManager
        }

        // Load the next level after the fade
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if a next level exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load!");
            
            SceneManager.LoadScene(0);
        }
    }
}
