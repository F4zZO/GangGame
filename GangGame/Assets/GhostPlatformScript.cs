using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatformScript : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] float disappearTime = 3;
    Animator myAnim;

    [SerializeField] bool canReset;
    [SerializeField] float resetTime;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myAnim.SetFloat("DisappearTime", 1 / disappearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
        {
            myAnim.SetBool("Trigger", true);
        }
    }

    public void TriggerReset()
    {
        if (canReset)
        {
            StartCoroutine(Reset());
        }

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(resetTime);
            myAnim.SetBool("Trigger", false);
        }
    }
}