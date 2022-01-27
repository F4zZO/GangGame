using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseScreen : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject TimerScreen;

    [SerializeField] private GameObject btwWin1;
    [SerializeField] private GameObject btwWin2;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            this.btwWin1.SetActive(false);
            this.btwWin2.SetActive(false);
        }
        GameManager.Instance.finish += ShowWinScreen;
        GameManager.Instance.lose += ShowLoseScreen;
    }

    private void OnDestroy()
    {
        GameManager.Instance.finish -= ShowWinScreen;
        GameManager.Instance.lose -= ShowLoseScreen;
    }

    public void ShowWinScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        this.WinScreen.SetActive(true);
        this.TimerScreen.SetActive(false);
    }

    public void ShowLoseScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        this.LoseScreen.SetActive(true);
        this.TimerScreen.SetActive(false);
    }

    public void PlayAgain()
    {
        GameManager.Instance.ReloadScene();
    }

    public void NextLevel()
    {
        GameManager.Instance.PlayNextLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.PlayMainMenu();
    }
}
