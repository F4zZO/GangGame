using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private GameObject[] coolHat;

    [SerializeField] private GameObject[] BtnObj;
    [SerializeField] private GameObject[] BtnX;

    [SerializeField] private GameObject Warning;

    [SerializeField] private GameObject[] HighScores;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        this.material.color = GameManager.Instance.playerColor;

        if (GameManager.Instance.playerHat > 0)
        {
            this.coolHat[GameManager.Instance.playerHat - 1].SetActive(true);
        }

        this.SetHatButton();

        this.SetScore();
    }

    public void PlayLevel1 ()
    {
        GameManager.Instance.PlayLevel1();
    }

    public void PlayLevel2()
    {
        GameManager.Instance.PlayLevel2();
    }

    public void PlayLevel3()
    {
        GameManager.Instance.PlayLevel3();
    }

    public void PlayTut()
    {
        GameManager.Instance.PlayTut();
    }


    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void EquipHat(int i)
    {
        this.Warning.SetActive(false);
        StopCoroutine(WarningTimer());

        if (i > 1)
        {
            if (!GameManager.Instance.hasHat[i - 2])
            {
                this.Warning.GetComponent<Text>().text = "GET 3 STARS IN LVL " + (i-1) + " TO OBTAIN";
                this.Warning.SetActive(true);
                StartCoroutine(WarningTimer());
                return;
            }
        }

        GameManager.Instance.playerHat = i;

        foreach (GameObject hat in this.coolHat)
        {
            hat.SetActive(false);
        }

        if (i == 0) return;

        this.coolHat[i - 1].SetActive(true);
    }

    IEnumerator WarningTimer()
    {
        yield return new WaitForSeconds(2f);
        this.Warning.SetActive(false);
    }

    private void SetHatButton()
    {
        for(int i = 0; i<3; i++)
        {
            this.BtnObj[i].SetActive(GameManager.Instance.hasHat[i]);
            this.BtnX[i].SetActive(!GameManager.Instance.hasHat[i]);
        }
    }

    private void SetScore()
    {
        for (int j = 0; j < 3; j++)
        {
            string minutes = ((int)GameManager.Instance.highScores[j] / 60).ToString();
            string seconds = (GameManager.Instance.highScores[j] % 60).ToString("f2");

            if (minutes.Equals("0"))
            {
                this.HighScores[j].GetComponent<Text>().text = string.Format("{0:00}", seconds);
            }
            else
            {
                this.HighScores[j].GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

        }
    }
}
