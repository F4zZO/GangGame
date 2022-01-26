using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormForce : MonoBehaviour
{
    [SerializeField] private float currentForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Rigidbody rig = other.gameObject.GetComponent<Rigidbody>();
            rig.AddForce(((this.transform.position - other.gameObject.transform.position).normalized) * this.currentForce);
        }
    }
}
