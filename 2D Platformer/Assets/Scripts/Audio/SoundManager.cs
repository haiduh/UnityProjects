using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } // Singleton class
    private AudioSource src;

    private void Awake() // Use Awake for initialisation
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist the first instance
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return; // Exit to prevent further initialization
        }

        src = GetComponent<AudioSource>(); // Initialize AudioSource
    }

    public void playSound(AudioClip sound)
    {
        src.PlayOneShot(sound); // Play the sound
    }
}
