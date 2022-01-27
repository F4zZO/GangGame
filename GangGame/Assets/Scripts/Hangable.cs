using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangable : MonoBehaviour
{
    [SerializeField] private bool attached = false;
    [SerializeField] private float speedLimit = 20;
    [SerializeField] private float swingAcceleration = 15;
    [SerializeField] private HingeJoint hJoint;

    private Rigidbody rb;
    private Rigidbody hangingRB;
    private float hJointMax = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (attached && Input.GetKey(KeyCode.W))
        {
            Debug.Log(hJoint.angle);
            rb.drag = -0.5f;
            float accelFactor = Mathf.Clamp(((hJointMax / 90) * swingAcceleration), 5, swingAcceleration);
            Vector3 forceAdd = rb.velocity * accelFactor;
            rb.AddForce(forceAdd, ForceMode.Acceleration);
        }
        else
        {
            rb.drag = 0.2f;
 
        }
        if (attached)
        {
            if(Mathf.Abs(hJoint.angle) > hJointMax)
            {
                hJointMax = Mathf.Abs(hJoint.angle);
                hJointMax = Mathf.Clamp(hJointMax, 0,90);
            }
        }

        if(rb.velocity.magnitude > speedLimit)
        {
            rb.velocity = rb.velocity.normalized * speedLimit;
        }
    }
    public void Attach(Vector3 dir,Rigidbody hangingRB)
    {
        this.hangingRB = hangingRB;
        if (!attached)
        {
            GetComponent<Rigidbody>().AddRelativeForce(dir * 750, ForceMode.Acceleration);
            attached = true;
        }
    }
    public void Detach()
    {
        attached = false;
        hangingRB.AddForce(Vector3.up * 25);
        this.hangingRB = null;
    }
}
