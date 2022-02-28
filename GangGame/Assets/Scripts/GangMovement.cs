using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GangMovement : MonoBehaviour
{
    [Header("--- INIT ---")]
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;
    [SerializeField] private Material material;
    [SerializeField] private GameObject[] coolHat;

    [Header("--- GENERAL ---")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private PlayerState playerState;

    [SerializeField] private bool isLocked = true;

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool hasJumpUp = false;
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

    private bool FallIsRunning = false; 
    private bool JumpIsRunning = false;

    private bool canHang = true;

    private Coroutine FallCoroutine;

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
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(0);
        }

        Cursor.lockState = CursorLockMode.Locked;

        this.hasJumpUp = true;

        GameManager.Instance.start += this.Unlock;
        GameManager.Instance.finish += this.Lock; 

        this.material.color = GameManager.Instance.playerColor;
        this.setHat(GameManager.Instance.playerHat);
    }

    private void OnDestroy()
    {
        GameManager.Instance.start -= this.Unlock;
        GameManager.Instance.finish -= this.Lock;
    }

    void Update()
    {
        if (this.isLocked) return;

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

        if (Input.GetKey(KeyCode.Space) && this.canMove && this.hasJumpUp && !this.isLanding)
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

        if(this.body.velocity.y > 0.1 && Input.GetKey(KeyCode.Space))
        {
            this.playerState = PlayerState.jump;
        }

        if (Input.GetKey(KeyCode.Space) && this.canHang)
        {
            checkForHangable();
        }
        else
        {
            this.transform.parent = null;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            this.GetComponent<Rigidbody>().isKinematic = false;     //Safety Net, if attaching to Rope doesn't work;
        }
        if (Input.GetKeyUp(KeyCode.Space) && this.isHanging)
        {
            StopHanging();
        }

        if (this.body.velocity.y < -0.5 && !this.isGrounded)
        {
            if (!this.FallIsRunning)
            {
                FallCoroutine = StartCoroutine(FallTime());
            }
            if (this.isFalling)
            {
                this.playerState = PlayerState.fall;
            }
        }
        else if (this.FallIsRunning)
        {
            StopCoroutine(FallCoroutine);
            this.FallIsRunning = false;
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
                    this.animator.Play("idle2");
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
        if (this.isLocked) return;

        if (this.body.velocity.y < 0)
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }
        else if (this.body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            this.body.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;
        }

        /*RaycastHit hits;

        if (Physics.//SphereCast(transform.position, 1f, Vector3.down, out hits, this.groundDistance) && this.body.velocity.y < 0.1)    
            Raycast(transform.position, Vector3.down, this.groundDistance) && this.body.velocity.y < 0.1)// && this.body.velocity.y > -0.5)
        {*/

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, Vector3.down, this.groundDistance);
        if (hits.Length > 2 && this.body.velocity.y < 0.5)
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
                StartCoroutine(LandTime());
            }

            if (!this.hasJumpUp && !this.JumpIsRunning)
            {
                StartCoroutine(JumpCD());
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
            this.body.AddForce(Vector3.up * 5 * Time.fixedDeltaTime, ForceMode.Impulse);

            

            this.hasJumpUp = false;
            this.doJump = false;
        }
    }

    private void checkForHangable()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, 2.5f, Vector3.forward, 5);
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
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (Vector3.Distance(this.transform.position, this.hangingTo.transform.position + Vector3.up * -2.35f) > 0.1f && !isHanging)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.hangingTo.transform.position + Vector3.up * -2.35f, 0.8f);
        }
        else
        {
            this.transform.parent = this.hangingTo.transform;
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            if (!isHanging)
            {
                this.hangingTo.Attach(direction, this.GetComponent<Rigidbody>());
            }
            this.isHanging = true;
            this.playerState = PlayerState.hang;
        }
    }

    public void StopHanging()
    {
        StartCoroutine(HangCD());
        this.isHanging = false;
        this.playerState = PlayerState.fall;
        this.hangingTo.Detach();
        this.hangingTo = null;
        this.transform.parent = null;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Unlock()
    {
        this.isLocked = false;
    }
    public void Lock()
    {
        this.isLocked = true;
        this.isTwerking = true;

        this.animator.Play("twerk");
    }

    private void setHat(int hat)
    {
        if (hat == 0) return;

        this.coolHat[hat - 1].SetActive(true);
    }


    IEnumerator JumpCD()
    {
        this.JumpIsRunning = true;

        yield return new WaitForSeconds(this.jumpCD);

        this.hasJumpUp = true;

        this.JumpIsRunning = false;
    }

    IEnumerator FallTime()
    {
        this.FallIsRunning = true;

        yield return new WaitForSeconds(this.fallTime);

        this.isFalling = true;

        this.FallIsRunning = false;
    }

    IEnumerator LandTime()
    {
        yield return new WaitForSeconds(this.landTime);

        this.isLanding = false;
    }

    IEnumerator HangCD()
    {
        this.canHang = false;
        yield return new WaitForSeconds(0.2f);
        this.canHang = true;
    }
}
