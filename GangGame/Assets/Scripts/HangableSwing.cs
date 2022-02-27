using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangableSwing : Hangable
{
    [SerializeField] private bool attached = false;
    [SerializeField] private HingeJoint hJoint;
    [SerializeField] private float swingDuration = 3;
    [SerializeField] private float angleIncrease = 5;
    [SerializeField] private float releaseAccelerationFactor = 1;
    [SerializeField] private float attachmentPush = 15;
    [SerializeField] private float attachmentPushForce = 20;

    private Rigidbody rb;
    private Rigidbody hangingRB;
    private float hJointMax = 0;
    private float swingDir = -1;
    private float elapsedTime = 0;
    private Vector3 oldpos;
    private Vector3 newpos;
    private Vector3 velocity;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        oldpos = transform.position;
    }
    private void FixedUpdate()
    {
        if (attached && Input.GetKey(KeyCode.W))
        {
            hJointMax += Time.deltaTime / swingDuration * angleIncrease;
            Debug.Log("Increase Swing");
        }
        else
        {
            hJointMax -= Time.deltaTime / swingDuration * 15;
        }
        hJointMax = Mathf.Clamp(hJointMax, 0, 90);

        JointSpring spr = hJoint.spring;
            float t = elapsedTime / swingDuration;
            t = t * t * (3f - 2f * t);
            spr.targetPosition = Mathf.Lerp(-swingDir*hJointMax, swingDir*hJointMax, t);
        hJoint.spring = spr;

        newpos = transform.position;
        var media = (newpos - oldpos);
        velocity = media / Time.deltaTime;
        oldpos = newpos;
        newpos = transform.position;

        elapsedTime += Time.deltaTime;
        if(elapsedTime > swingDuration)
        {
            elapsedTime = 0;
            swingDir *= -1;
        }
    }

    public override void Attach(Vector3 dir,Rigidbody hangingRB)
    {
        rb.AddForce(dir * attachmentPushForce);
        this.hangingRB = hangingRB;
        attached = true;
        hJointMax += attachmentPush;
        swingDir = -1;
        elapsedTime = swingDuration/2;
    }
    public override void Detach()
    {
        attached = false;
        hangingRB.AddForce(Vector3.up * 25);
        hangingRB.velocity = velocity*releaseAccelerationFactor;
        this.hangingRB = null;
    }
}
