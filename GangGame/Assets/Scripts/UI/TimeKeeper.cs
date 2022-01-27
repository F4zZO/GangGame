using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private float startTime;

    private bool isStarted = false;

    private void Start()
    {
        GameManager.Instance.start += StartTimer;
    }

    private void OnDestroy()
    {
        GameManager.Instance.start -= StartTimer;
    }

    void Update()
    {
        if (!this.isStarted) return;

        float time = Time.time - startTime;

        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");

        if (minutes.Equals("0"))
        {
            this.timerText.text = string.Format("{0:00}", seconds);
        }
        else
        {
            this.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        this.isStarted = true;
        this.startTime = Time.time;
    }
}