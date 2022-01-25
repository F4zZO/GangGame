using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] private Toggle t;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
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

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
        
   
}
