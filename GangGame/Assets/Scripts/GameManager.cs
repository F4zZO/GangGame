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

    public Data data;

    public Color playerColor; 
    public int playerHat;

    public bool[] hasHat;
    public float[] highScores;

    public float[ , ] scoreTime;

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
        this.scoreTime = new float[3, 2];

        this.scoreTime[0, 0] = 80;
        this.scoreTime[0, 1] = 40;

        this.scoreTime[1, 0] = 80;
        this.scoreTime[1, 1] = 50;

        this.scoreTime[2, 0] = 240;
        this.scoreTime[2, 1] = 120;


        this.isPaused = false;
        this.state = GameState.run;

        this.hasHat = new bool[3];
        this.highScores = new float[3];

        this.data = SaveSystem.LoadData();

        if(this.data == null)
        {
            SaveSystem.SaveData(this);
        }

        this.playerHat = this.data.selectedHat;

        this.playerColor.r = this.data.selectedColor[0];
        this.playerColor.g = this.data.selectedColor[1];
        this.playerColor.b = this.data.selectedColor[2];

        this.hasHat = this.data.hasHats;

        this.highScores = this.data.highScore;
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

    public void SaveGame()
    {
        SaveSystem.SaveData(this);
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

        SaveSystem.SaveData(this);
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
            case 3:
                PlayTut();
                break;
            default:
                PlayMainMenu();
                break;
        }
    }

    public void PlayLevel1()
    {
        this.SaveGame();
        Time.timeScale = 1f;
        this.lvl = 1;
        this.state = GameState.start;
        SceneManager.LoadScene(1);
    }

    public void PlayLevel2()
    {
        this.SaveGame();
        Time.timeScale = 1f;
        this.lvl = 2;
        this.state = GameState.start;
        SceneManager.LoadScene(2);
    }

    public void PlayLevel3()
    {
        this.SaveGame();
        Time.timeScale = 1f;
        this.lvl = 3;
        this.state = GameState.start;
        SceneManager.LoadScene(3);
    }

    public void PlayTut()
    {
        this.SaveGame();
        Time.timeScale = 1f;
        this.lvl = 4;
        this.state = GameState.start;
        SceneManager.LoadScene(4);
    }

    private void RateRun()
    {
        if (this.lvl == 4) return;

        if (this.time < this.highScores[(int) this.lvl - 1] || this.highScores[(int)this.lvl - 1] == 0){
            this.highScores[(int)this.lvl - 1] = this.time;
        }

        this.rating = 1;
        if (this.time > this.scoreTime[(int)this.lvl-1, 0]) return;
        this.rating = 2;
        if (this.time > this.scoreTime[(int)this.lvl - 1, 1]) return;
        this.rating = 3;

        this.hasHat[(int)this.lvl - 1] = true;
    }
}