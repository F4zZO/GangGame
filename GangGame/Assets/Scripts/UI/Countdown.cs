using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private GameObject CountdownUI;
    [SerializeField] private GameObject Header;
    [SerializeField] private GameObject CountdownTxt;

    private float waitTime = 2;
    private int countdown = 3;

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        GameManager.Instance.state = GameManager.GameState.start;

        while (this.waitTime > 0)
        {
            yield return new WaitForSeconds(1f);

            this.waitTime--;
        }

        this.Header.SetActive(false);
        this.CountdownTxt.SetActive(true);

        while (this.countdown > 0)
        {
            this.CountdownTxt.GetComponent<TextMeshProUGUI>().text = this.countdown.ToString();

            yield return new WaitForSeconds(1f);

            this.countdown--;
        }

        this.CountdownTxt.GetComponent<TextMeshProUGUI>().text = "GO";

        GameManager.Instance.state = GameManager.GameState.run;

        yield return new WaitForSeconds(1f);

        this.CountdownUI.SetActive(false);
    }
}