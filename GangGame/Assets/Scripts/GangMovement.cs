using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangMovement : MonoBehaviour
{
    [Header("--- INIT ---")]
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform cam;
    [SerializeField] private Animator anim;

    [Header("--- GENERAL ---")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 15f;

    [SerializeField] private float currentSpeed;
    [SerializeField] private PlayerState playerState;

    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private bool isGrounded = false;

    //---------------------------------------------------------
    private float turnSmoothVelocity;
    private Vector3 moveDir;

    private enum PlayerState
    {
        idle,
        walk,
        run,
        jump,
        fastJump,
        fall,
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        this.moveDir = Vector3.zero;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (this.playerState != PlayerState.fall || this.playerState != PlayerState.jump)
        {
            this.isGrounded = true;
        }
        else
        {
            this.isGrounded = false;
        }


        if (direction.magnitude < 0.1f && this.isGrounded)
        {
            this.playerState = PlayerState.idle;
        }

        if (direction.magnitude >= 0.1f && this.isGrounded)
        {
            this.playerState = PlayerState.walk;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                this.playerState = PlayerState.run;
            }
        }

        if (Input.GetKey(KeyCode.Space) && this.isGrounded)
        {
            this.playerState = PlayerState.jump;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.playerState = PlayerState.fastJump;
            }
        }

        switch (this.playerState)
        {
            case PlayerState.idle:
                this.currentSpeed = 0;
                this.anim.SetTrigger("Idle");
                break;
            case PlayerState.walk:
                this.currentSpeed = this.walkSpeed;
                this.anim.SetTrigger("Walk");
                break;
            case PlayerState.run:
                this.currentSpeed = this.runSpeed;
                this.anim.SetTrigger("Run");
                break;
            case PlayerState.jump:
                this.anim.SetTrigger("Jump");
                break;
            case PlayerState.fastJump:
                this.anim.SetTrigger("FastJump");
                break;
        }

        //if (Input.GetKey(KeyCode.LeftShift)) this.isSprinting = true;

        //if (Input.GetKey(KeyCode.Space)) this.isJumping = true;

        //if (direction.magnitude < 0.1f) this.isIdle = true;
        //else if (Input.GetKey(KeyCode.LeftShift)) this.isSprinting = true;

        /*
        if (Input.GetKey(KeyCode.LeftShift)) this.state = PlayerState.sprinting;
        else if (direction.magnitude < 0.1f) this.state = PlayerState.idle;
        else this.state = PlayerState.walking;
        

        if (!this.isIdle)   //direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + this.cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, this.turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            this.moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            this.moveDir.Normalize();

            //this.body.MovePosition(this.transform.position + this.moveDir * Time.deltaTime * this.currentSpeed);
        }

        if (this.isIdle) this.currentSpeed = 0;
         else if (this.isSprinting) this.currentSpeed = this.sprint;
          else this.currentSpeed = this.walk;



        //switch()


        /*
        switch (this.state)
        {
            case PlayerState.idle:
                this.currentSpeed = 0;
                this.anim.SetTrigger("Idle");
                break;
            case PlayerState.walking:
                this.currentSpeed = this.walk;
                this.anim.SetTrigger("Walk");
                break;
            case PlayerState.sprinting:
                this.currentSpeed = this.sprint;
                this.anim.SetTrigger("Sprint");
                break;
        }

        this.body.MovePosition(this.transform.position + this.moveDir * Time.deltaTime * this.currentSpeed);

        if (Input.GetKey(KeyCode.Space) && this.state != PlayerState.airial)
        {
            this.state = PlayerState.airial;
            Vector3 v = new Vector3(0f, 20f, 0f);
            this.body.MovePosition(this.transform.position + v * Time.deltaTime);
        }

        */
    }
}
