using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScreen : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1f;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
