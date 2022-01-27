using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            //coll.gameObject.GetComponent<PlayerUI>().showWinScreen();
            GameManager.Instance.Lose();
            Time.timeScale = 0f;
        }
    }
}
