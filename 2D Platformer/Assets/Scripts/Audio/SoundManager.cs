using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } // Singleton class
    private AudioSource src;
    private AudioSource musicSrc;

    private void Awake() // Use Awake for initialisation
    {
        src = GetComponent<AudioSource>(); // Initialize AudioSource
        musicSrc = transform.GetChild(0).GetComponent<AudioSource>();

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

        //Assign the initial volumes.
        changeMusicVolume(0);
        changeSoundVolume(0);
    }

    public void playSound(AudioClip sound)
    {
        src.PlayOneShot(sound); // Play the sound
    }

    public void changeSoundVolume(float change)
    {
        changeSourceVolume(1, "soundVolume", change, src);
    }

    private void changeSourceVolume (float baseVolume, string volumeName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //Save the new sound volume.
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
    public void changeMusicVolume(float change)
    {
        changeSourceVolume(0.3f, "musicVolume", change, musicSrc);
    }

    public void changeBackgroundMusic(AudioClip newMusic)
    {
        if (musicSrc.isPlaying)
        {
            musicSrc.Stop();
        }

        musicSrc.clip = newMusic;
        musicSrc.Play();
    }


}
