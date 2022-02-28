using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Finish : MonoBehaviour
{

    [SerializeField] private GameObject Firework;
    [SerializeField] private GameObject Music;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            this.Firework.SetActive(true);
            this.gameObject.GetComponent<AudioSource>().Play();
            this.Music.GetComponent<AudioSource>().Stop();
            GameManager.Instance.Win();
        }
    }
}
