using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }*/
    
    

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("now");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("now1");
            col.gameObject.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        Debug.Log("out");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("out1");
            col.gameObject.transform.parent = null;
        }
    }
}
