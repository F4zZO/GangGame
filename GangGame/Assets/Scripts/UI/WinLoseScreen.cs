using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class WinLoseScreen : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject TimerScreen;

    [SerializeField] private GameObject btwWin1;
    [SerializeField] private GameObject btwWin2;
    [SerializeField] private GameObject txtTime;

    [SerializeField] private GameObject[] Stars;
    [SerializeField] private GameObject Unlock;
    [SerializeField] private GameObject UnlockImage;


    [SerializeField] private Sprite[] UnlockSprites;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
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

        string minutes = ((int)GameManager.Instance.time / 60).ToString();
        string seconds = (GameManager.Instance.time % 60).ToString("f2");

        if (minutes.Equals("0"))
        {
            this.txtTime.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}", seconds);
        }
        else
        {
            this.txtTime.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if(GameManager.Instance.rating == 3)
        {
            //this.UnlockImage.GetComponent<Image>().sprite = this.UnlockSprites[(int)GameManager.Instance.lvl];
            this.Unlock.SetActive(true);
        }
        else
        {
            this.Unlock.SetActive(false);
        }

        this.WinScreen.SetActive(true);
        this.TimerScreen.SetActive(false);

        this.ShowStars();
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

    private void ShowStars()
    {
        this.Stars[0].GetComponent<Animator>().SetTrigger("Start");
        if (GameManager.Instance.rating < 2) return;
        this.Stars[1].GetComponent<Animator>().SetTrigger("Start2");
        if (GameManager.Instance.rating < 3) return;
        this.Stars[2].GetComponent<Animator>().SetTrigger("Start3");
    }
}
