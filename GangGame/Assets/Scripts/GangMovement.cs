using System.Timers;
using UnityEngine;

public class GangMovement : MonoBehaviour
{
    [Header("--- INIT ---")]
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider spring;

    [Header("--- GENERAL ---")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private PlayerState playerState;

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool canJump = false;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private bool isLanding = false;
    [SerializeField] private bool isTwerking = false;
    [SerializeField] private bool isHanging = false;
    [SerializeField] private Hangable hangingTo = null;

    [SerializeField] private bool doJump = false;
    [SerializeField] private bool fastJump = false;

    [Header("--- STATS ---")]

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float airFastSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float landSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;

    [SerializeField] private float groundDistance;

    [SerializeField] private float jumpCD;

    [SerializeField] private float fallTime;
    [SerializeField] private float landTime;

    [SerializeField] private float turnSmoothTime = 0.1f;

    //---------------------------------------------------------
    private float turnSmoothVelocity;
    private Vector3 moveDir;
    private Vector3 direction;
    private PlayerState lastState;

    private Timer timerJumpCD;
    private Timer timerFall;
    private Timer timerLand;

    private enum PlayerState
    {
        idle,
        walk,
        run,
        jump,
        fall,
        land,
        standUp,
        twerk,
        ragdoll,
        hang
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        /*
        try
        {
            if (GameManager.Instance.isRagdoll)
            {
                this.gameObject.SetActive(false);
            }
        }
        catch { } */

        this.timerJumpCD = new Timer();
        this.timerJumpCD.Elapsed += this.EndJumpCD;

        this.timerFall = new Timer();
        this.timerFall.Elapsed += this.EndFall;

        this.timerLand = new Timer();
        this.timerLand.Elapsed += this.EndLand;

        this.canJump = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        this.moveDir = Vector3.zero;
        this.direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (this.direction.magnitude < 0.1f && this.canMove)
        {
            this.playerState = PlayerState.idle;
            this.currentSpeed = 0;
        }

        if (this.direction.magnitude >= 0.1f && this.canMove && !this.isHanging)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.playerState = PlayerState.run;
                this.currentSpeed = this.runSpeed;
            }
            else
            {
                this.playerState = PlayerState.walk;
                this.currentSpeed = this.walkSpeed;
            }
        }

        if (Input.GetKey(KeyCode.Space) && this.canMove && this.canJump && !this.isLanding)
        {
            this.doJump = true;
            this.playerState = PlayerState.jump;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.fastJump = true;
            }
            else
            {
                this.fastJump = false;
            }
        }

        if (this.body.velocity.y > 0.1 && Input.GetKey(KeyCode.Space))
        {
            this.playerState = PlayerState.jump;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            checkForHangable();
        }
        else
        {
            this.GetComponent<Rigidbody>().isKinematic = false;     //Safety Net, if attaching to Rope doesn't work;
        }
        if (Input.GetKeyUp(KeyCode.Space) && this.isHanging)
        {
            stopHanging();
        }

        if (this.body.velocity.y < -0.5 && !this.isGrounded)
        {
            if (!this.timerFall.Enabled)
            {
                this.timerFall.Interval = this.fallTime;
                this.timerFall.Start();
            }
            if (this.isFalling)
            {
                this.playerState = PlayerState.fall;
            }
        }
        else
        {
            this.timerFall.Stop();
        }
        if (this.isHanging)
        {
            this.playerState = PlayerState.hang;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTwerking = !isTwerking;
        }

        if (this.isTwerking && this.direction.magnitude < 0.1f && this.body.velocity.y < 1)
        {
            this.playerState = PlayerState.twerk;
        }
        else if (this.isTwerking)
        {
            this.isTwerking = false;
        }

        if (this.isLanding)
        {
            this.playerState = PlayerState.land;
        }

        if (this.playerState != this.lastState)
        {
            switch (this.playerState)
            {
                case PlayerState.idle:
                    this.animator.Play("idle");
                    break;
                case PlayerState.walk:
                    this.animator.Play("walk");
                    break;
                case PlayerState.run:
                    this.animator.Play("run");
                    break;
                case PlayerState.jump:
                    this.animator.Play("jump");
                    break;
                case PlayerState.fall:
                    this.animator.Play("fall");
                    break;
                case PlayerState.land:
                    this.animator.Play("land");
                    break;
                case PlayerState.twerk:
                    this.animator.Play("twerk");
                    break;
                case PlayerState.hang:
                    this.animator.Play("hangingIdle");
                    break;

            }
        }

        this.lastState = this.playerState;
    }

    void FixedUpdate()
    {
        if (this.body.velocity.y < 0)
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }
        else if (this.body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }

        if (Physics.Raycast(transform.position, Vector3.down, this.groundDistance) && this.body.velocity.y < 0.1)// && this.body.velocity.y > -0.5)
        {
            this.isGrounded = true;

            if (this.isGrounded)
            {
                this.canMove = true;
            }

            if (this.isFalling)
            {
                this.isFalling = false;
                this.isLanding = true;
                this.timerLand.Interval = this.landTime;
                this.timerLand.Start();
            }

            if (!this.canJump && !this.timerJumpCD.Enabled)
            {
                this.timerJumpCD.Interval = this.jumpCD;
                this.timerJumpCD.Start();
            }
        }
        else
        {
            this.isGrounded = false;
            this.canMove = false;
            this.currentSpeed = this.airSpeed;

            if (this.fastJump)
            {
                this.currentSpeed = this.airFastSpeed;
            }

            if (this.isFalling)
            {
                this.currentSpeed = this.fallSpeed;
            }
        }

        if (this.isLanding)
        {
            this.currentSpeed = this.landSpeed;
        }

        if (this.direction.magnitude >= 0.1f && !this.isHanging)
        {
            float targetAngle = Mathf.Atan2(this.direction.x, this.direction.z) * Mathf.Rad2Deg + this.cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, this.turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            this.moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            this.moveDir.Normalize();

            this.body.MovePosition(this.transform.position + this.moveDir * Time.deltaTime * this.currentSpeed);
        }

        if (this.doJump)
        {
            this.body.velocity = Vector3.up * this.jumpForce;

            this.canJump = false;
            this.doJump = false;
        }
    }

    public void EndJumpCD(object source, ElapsedEventArgs e)
    {
        this.canJump = true;
        this.timerJumpCD.Stop();
    }

    public void EndFall(object source, ElapsedEventArgs e)
    {
        this.isFalling = true;
        this.timerFall.Stop();
    }

    public void EndLand(object source, ElapsedEventArgs e)
    {
        this.isLanding = false;
        this.timerLand.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Death")
        {
            try
            {
                GameManager.Instance.ReloadScene();
            }
            catch
            { }
        }

        if (collision.collider.tag == "Win")
        {
            try
            {
                GameManager.Instance.Win();
            }
            catch
            { }
        }
    }

    /*
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Floor")
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
    }

    private void OnCollisionExit(Collision collision)
    {
    }

    private void OnCollisionStay(Collision collision)
    {
    }

    if (this.playerState != PlayerState.fall && this.playerState != PlayerState.ragdoll && this.isGrounded)
    {
        this.canMove = true;
    }
    else
    {
        this.canMove = false;
    }       
     */
    private void checkForHangable()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, 1, Vector3.forward, 5);
        foreach (RaycastHit hit in hits)
        {
            Hangable tryHang = hit.transform.gameObject.GetComponent<Hangable>();
            if (tryHang != null)
            {
                hangTo(tryHang);
                return;
            }
        }
        if (this.hangingTo != null)
        {
            hangTo(this.hangingTo);
        }
    }

    private void hangTo(Hangable tryHang)
    {
        this.hangingTo = tryHang;
        Rigidbody objRigidBody = this.hangingTo.GetComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (Vector3.Distance(this.transform.position, this.hangingTo.transform.position + Vector3.up * -2f) > 0.1f && !isHanging)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.hangingTo.transform.position + Vector3.up * -2f, 0.8f);
        }
        else
        {
            if (this.gameObject.GetComponent<FixedJoint>() == null)
                this.gameObject.AddComponent<FixedJoint>().connectedBody = objRigidBody;

            this.hangingTo.Attach(direction,this.GetComponent<Rigidbody>());
            this.isHanging = true;
            this.playerState = PlayerState.hang;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    } 

    private void stopHanging()
    {
        this.isHanging = false;
        this.playerState = PlayerState.fall;
        this.hangingTo.Detach();
        this.hangingTo = null;
        Destroy(this.GetComponent<FixedJoint>());
    }

}
