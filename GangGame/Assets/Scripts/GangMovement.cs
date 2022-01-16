using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangMovement : MonoBehaviour
{
    public Rigidbody body;
    public Transform cam;
    public Animator anim;
    public float walk = 3f;
    public float sprint = 15f;
    public float speed;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDir = Vector3.zero;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprint;
        }
        else
        {
            speed = walk;
        }

        if (direction.magnitude >= 0.1f)
        {
            //ToggleWalkingAnimation(true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();
        }
        else
        {
            speed = 0f;
        }

        body.MovePosition(transform.position + moveDir * Time.deltaTime * speed);


        anim.SetFloat("Speed", this.speed);
    }
}
