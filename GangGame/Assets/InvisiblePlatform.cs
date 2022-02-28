using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    private bool on = true;

    private void Start()
    {
        StartCoroutine(OnOffTimer());
    }

    IEnumerator OnOffTimer()
    {
        yield return new WaitForSeconds(2f);
        this.on = !this.on;
        this.gameObject.SetActive(this.on);
    }
}
