using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    private void Awake()
    { 
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void playGame()
    { 
        SceneManager.LoadScene(1);
    }

    public void stopGame()
    {
        Application.Quit();
    }

    public void soundVolume()
    {
        SoundManager.instance.changeSoundVolume(0.2f);
    }

    public void musicVolume()
    {
        SoundManager.instance.changeMusicVolume(0.2f);
    }
}
