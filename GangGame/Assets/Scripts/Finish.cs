using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            coll.gameObject.GetComponent<PlayerUI>().showWinScreen();
            //GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => x.SetActive(false));
            Time.timeScale = 0f;
        }
    }
}
