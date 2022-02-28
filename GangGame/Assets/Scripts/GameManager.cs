using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float lvl = 0;
    public int rating = 0;
    public float time;
    public bool isPaused;
    public delegate void Delegate();
    public Delegate start;
    public Delegate finish;
    public Delegate lose;
    public Delegate getTime;

    public Color playercolor;

    private GameState state;

    public enum GameState
    {
        start,
        run,
        finish
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        this.isPaused = false;
        this.state = GameState.run;
    }

    void Update()
    {
        
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        this.state = GameState.start;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartPlay()
    {
        this.start();
    }

    public void Win()
    {
        this.state = GameState.finish;
        this.getTime();
        this.RateRun();
        this.finish();
    }

    public void Lose()
    {
        this.state = GameState.finish;
        this.lose();
    }

    public void PlayMainMenu()
    {
        Time.timeScale = 1f;
        this.lvl = 0;
        SceneManager.LoadScene(0);
    }

    public void PlayNextLevel()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                PlayLevel1();
                break;
            case 1:
                PlayLevel2();
                break;
            case 2:
                PlayLevel3();
                break;
            default:
                PlayMainMenu();
                break;
        }
    }

    public void PlayLevel1()
    {
        Time.timeScale = 1f;
        this.lvl = 1;
        this.state = GameState.start;
        SceneManager.LoadScene(1);
    }

    public void PlayLevel2()
    {
        Time.timeScale = 1f;
        this.lvl = 2;
        this.state = GameState.start;
        SceneManager.LoadScene(2);
    }

    public void PlayLevel3()
    {
        Time.timeScale = 1f;
        this.lvl = 3;
        this.state = GameState.start;
        SceneManager.LoadScene(3);
    }

    private void RateRun()
    {
        this.rating = 1;
        if (this.time > 60f) return;
        this.rating = 2;
        if (this.time > 30f) return;
        this.rating = 3;
    }
}