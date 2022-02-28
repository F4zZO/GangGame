using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameManager gm;

    private bool isLocked = true;

    private void Start()
    {
        this.gm = GameManager.Instance;
    }

    void Update()
    {
        this.gm.start += this.UnLock;
        this.gm.finish += this.Lock;
        this.gm.lose += this.Lock;

        if (Input.GetKeyDown(KeyCode.Escape) && !this.isLocked)
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

    private void OnDestroy()
    {
        this.gm.start -= this.UnLock;
        this.gm.finish -= this.Lock;
        this.gm.lose -= this.Lock;
    }

    public void Lock()
    {
        this.isLocked = true;
    }

    public void UnLock()
    {
        this.isLocked = false;
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
