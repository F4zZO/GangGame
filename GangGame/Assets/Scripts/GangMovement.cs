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
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float fallForce;

    [SerializeField] private float currentSpeed;
    [SerializeField] private PlayerState playerState;

    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool canMove = false;

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
        standUp,
        ragdoll
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

        //Vector3 jumpV = new Vector3(0f, this.jumpForce, 0f).normalized;

        if (this.body.velocity.y < 0)
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }
        else if(this.body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }

        if (this.playerState != PlayerState.fall && this.playerState != PlayerState.ragdoll && this.isGrounded)
        {
            this.canMove = true;
        }
        else
        {
            this.canMove = false;
        }

        if (direction.magnitude < 0.1f && this.canMove)
        {
            this.playerState = PlayerState.idle;
        }

        if (direction.magnitude >= 0.1f && this.canMove)
        {
            this.playerState = PlayerState.walk;
            this.currentSpeed = this.walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.playerState = PlayerState.run;
                this.currentSpeed = this.runSpeed;
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + this.cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, this.turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            this.moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            this.moveDir.Normalize();


            this.body.MovePosition(this.transform.position + this.moveDir * Time.deltaTime * this.currentSpeed);

            //this.body.AddForce(this.moveDir * Time.deltaTime * this.currentSpeed);
            //this.body.velocity = this.moveDir * Time.deltaTime * this.currentSpeed * 100;
        }

        if (Input.GetKey(KeyCode.Space) && this.canMove)
        {
            this.playerState = PlayerState.jump;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.playerState = PlayerState.fastJump;
            }

            this.body.velocity = Vector3.up * this.jumpForce;
            //this.body.AddForce
        }



        switch (this.playerState)
        {
            case PlayerState.idle:
                this.currentSpeed = 0;
                this.anim.SetTrigger("Idle");
                break;
            case PlayerState.walk:
                this.anim.SetTrigger("Walk");
                break;
            case PlayerState.run:
                this.anim.SetTrigger("Run");
                break;
            case PlayerState.jump:
                //this.anim.SetTrigger("Jump");
                this.anim.SetTrigger("Idle");
                if (this.canMove)
                {
                    this.playerState = PlayerState.idle;
                }
                break;
            case PlayerState.fastJump:
                //this.anim.SetTrigger("FastJump");
                this.anim.SetTrigger("Idle");
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "a")
        {
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            //this.isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            this.isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            this.isGrounded = true;
        }
    }
}
