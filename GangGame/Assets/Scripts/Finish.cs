using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Finish : MonoBehaviour
{

    [SerializeField] private GameObject Firework;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            this.Firework.SetActive(true);
            GameManager.Instance.Win();
        }
    }
}
