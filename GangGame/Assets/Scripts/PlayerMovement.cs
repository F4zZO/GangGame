using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform cam;
    public float speed = 6f;
    public float gravity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;
    // Update is called once per frame

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

        if(direction.magnitude >= 0.1f)
        {
            ToggleWalkingAnimation(true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle,0f) * Vector3.forward;
            moveDir.Normalize();
        }
        else{
            ToggleWalkingAnimation(false);
        }
        handleGravity();
        controller.Move(moveDir * speed * Time.deltaTime);
    }

    void handleGravity()
    {
        if (controller.isGrounded)
        {
            moveDir.y = -.5f ;
        }
        else
        {
            moveDir.y -= gravity;
        }
    }

    void ToggleWalkingAnimation(bool animate)
    {
        bool isWalking = animator.GetBool("isWalking");
        if (animate) {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}


