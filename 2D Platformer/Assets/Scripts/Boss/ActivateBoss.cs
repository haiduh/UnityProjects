using UnityEngine;
using UnityEngine.UI;

public class ActivateBoss : MonoBehaviour
{
    [Header("References")]
    public GameObject Boss;      // Reference to the Boss GameObject
    public Slider HealthBar;     // Reference to the Health Bar UI
    public AudioClip bossMusic;  // The music clip to play during the boss fight

    private void Start()
    {
        // Ensure the health bar is disabled at the start
        if (HealthBar != null)
        {
            HealthBar.gameObject.SetActive(false);
        }

        // Ensure the boss is disabled at the start
        if (Boss != null)
        {
            Boss.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if Player entered the trigger
        {
            // Activate the boss
            if (Boss != null)
            {
                Boss.SetActive(true);
            }

            // Enable the health bar UI
            if (HealthBar != null)
            {
                HealthBar.gameObject.SetActive(true);
            }

            // Play the boss music
            if (SoundManager.instance != null && bossMusic != null)
            {
                SoundManager.instance.changeBackgroundMusic(bossMusic); // Play boss music
            }

            // Disable this trigger object
            gameObject.SetActive(false);
        }
    }
}
