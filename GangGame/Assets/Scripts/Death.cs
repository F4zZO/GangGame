using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    [SerializeField] private GameObject Music;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            this.Music.GetComponent<AudioSource>().Stop();
            GameManager.Instance.Lose();
            Time.timeScale = 0f;
        }
    }
}
