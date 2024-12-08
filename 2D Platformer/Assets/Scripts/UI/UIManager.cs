using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            //Pause active, pause not active

            if(pauseScreen.activeInHierarchy)
            PauseGame(false);
            else PauseGame(true);  
        }
    }

    #region GameOver
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.playSound(gameOverSound);
 
        Time.timeScale = 0;
    }


    public void Restart()
    {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
        Application.Quit();
    }
    #endregion


    #region Pause
    public void PauseGame(bool pause)
    {
        //if pause is true, set pause || pause = false, unpause
        pauseScreen.SetActive(pause);

        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void soundVolume()
    {
        SoundManager.instance.changeSoundVolume(0.2f);
    }

    public void musicVolume()
    {
        SoundManager.instance.changeMusicVolume(0.2f);
    }
    #endregion
}
