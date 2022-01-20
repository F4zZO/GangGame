using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isRagdoll;

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

    public void PlayLevel1(bool rag)
    {
        SceneManager.LoadScene(1);
        this.isRagdoll = rag;
    }

    public void PlayLevel2(bool rag)
    {
        SceneManager.LoadScene(2);
        this.isRagdoll = rag;
    }

    public void PlayLevel3(bool rag)
    {
        SceneManager.LoadScene(3);
        this.isRagdoll = rag;
    }
}