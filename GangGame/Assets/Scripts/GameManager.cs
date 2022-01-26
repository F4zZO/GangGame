using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isPaused;
    public GameState state;

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
        this.state = GameState.start;
    }

    void Update()
    {
        
    }

    public void ReloadScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }
    public void Win()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayLevel1()
    {
        this.state = GameState.start;
        SceneManager.LoadScene(1);
    }

    public void PlayLevel2()
    {
        this.state = GameState.start;
        SceneManager.LoadScene(2);
    }

    public void PlayLevel3()
    {
        this.state = GameState.start;
        SceneManager.LoadScene(3);
    }
}