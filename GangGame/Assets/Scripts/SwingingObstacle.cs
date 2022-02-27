using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingObstacle : MonoBehaviour
{
    [SerializeField] private HingeJoint hJoint;
    [SerializeField] private float swingDuration = 3;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float force = 10;
    private float swingDir = -1;
    private float elapsedTime = 0;

    private void FixedUpdate()
    {

        JointSpring spr = hJoint.spring;
            float t = elapsedTime / swingDuration;
            t = t * t * (3f - 2f * t);
            spr.targetPosition = Mathf.Lerp(-swingDir * maxAngle, swingDir * maxAngle, t);
        hJoint.spring = spr;

        elapsedTime += Time.deltaTime;
        if (elapsedTime > swingDuration)
        {
            elapsedTime = 0;
            swingDir *= -1;
        }
    }
}

