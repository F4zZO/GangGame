using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;

    private GameManager gm = GameManager.Instance;

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.gm.isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        this.gm.isPaused = false;
        this.GetComponent<AudioSource>().UnPause();
    }
    public void Pause ()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        this.gm.isPaused = true;
        this.GetComponent<AudioSource>().Pause();
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
   
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
