using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Toggle t;
   
    public void PlayLevel1 ()
    {

        GameManager.Instance.PlayLevel1(t.isOn);
    }

    public void PlayLevel2()
    {
        GameManager.Instance.PlayLevel2(t.isOn);
    }

    public void PlayLevel3()
    {
        GameManager.Instance.PlayLevel3(t.isOn);
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
        
   
}
