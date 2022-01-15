using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMovement : MonoBehaviour
{
    public Rigidbody hip;
    public ConfigurableJoint hipJoint;
    public Transform cam;
    public float speed = 6f;
    public float gravity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDir = Vector3.zero;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //moveDir.Normalize();
            this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            this.hip.AddForce(direction * this.speed);
        }
    }
}
