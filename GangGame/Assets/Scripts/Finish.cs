using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            //coll.gameObject.GetComponent<PlayerUI>().showWinScreen();
            GameManager.Instance.Win();
            Time.timeScale = 0f;
        }
    }
}
