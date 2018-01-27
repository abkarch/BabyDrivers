using UnityEngine;
using System.Collections;

public class PhysicsPlayerController : MonoBehaviour
{
    int playerNum = 1;
    public Rigidbody myRigidbody;
    public float gravity = 10;
    //limits the y velocity of the player, so the player does not get flung upwards
    public float maxYVelocity = 2f;

    public float walkSpeed = 1f;

    public float turnSmoothing = 3.0f;

    public float speedDampTime = 0.1f;
    public float autoAimMaxAngleOffset = 10f;
    
    public float jumpVelocity = 15.0f;
    public float jumpCooldown = 0.3f;

    private float timeToNextJump = 0;

    private float speed;

    private Vector3 lastDirection;

    private Animator anim;
    private int groundedBool;
    public Transform cameraTransform;

    private float h;
    private float v;

    private bool aim;
    private bool rightAim = false;

    private bool run;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    private float distToGround;

    private bool isGrounded = false;

    void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (myRigidbody == null)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        groundedBool = Animator.StringToHash("OnGround");
        //distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Start()
    {
    }

    bool CalculateIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        return isGrounded;
    }

    void Update()
    {
        if (Time.timeScale <= 0.01f)
        {
            return;
        }
        
        h = Input.GetAxis("HorizontalP" + playerNum);
        v = Input.GetAxis("VerticalP" + playerNum);

        isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1 || run;
    }

    void FixedUpdate()
    {
        anim.SetBool(groundedBool, CalculateIsGrounded());
        
        MovementManagement(h, v, run);

        JumpManagement();
    }

    void JumpManagement()
    {
        if (myRigidbody.velocity.y < 10) // already jumped
        {
            if (timeToNextJump > 0)
                timeToNextJump -= Time.deltaTime;
        }
        if (isGrounded)
        {
            if (Input.GetButtonDown("JumpP" + playerNum))
            {
                if (speed > 0 && timeToNextJump <= 0 && !aim)
                {
                    myRigidbody.velocity = new Vector3(0, jumpVelocity, 0);
                    timeToNextJump = jumpCooldown;
                }
            }
            isGrounded = false;
        }
        if (!isGrounded)
        {
            anim.SetFloat("Jump", myRigidbody.velocity.y);
        }
    }

    void MovementManagement(float horizontal, float vertical, bool running)
    {
        Rotating(horizontal, vertical);

        if (isMoving)
        {
            speed = walkSpeed;
        }
        else
        {
            speed = 0f;
            if (myRigidbody.velocity.y.AlmostEquals(0, .1f))
            {
                myRigidbody.velocity = Vector3.zero;
            }
        }

        anim.SetFloat("Forward", speed, 0.1f, Time.deltaTime);

        if (isMoving)
        {
            Vector3 targetVelocity = speed * transform.forward;
            Vector3 currVelocity = myRigidbody.velocity;
            
            Vector3 forward;

            if (cameraTransform != null)
            {
                forward = cameraTransform.TransformDirection(Vector3.up);
            }
            else
            {
                forward = GameManager.instance.ActiveCamera.transform.TransformDirection(Vector3.up);
            }
            forward.y = 0.0f;

            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 dir = forward * vertical + right * horizontal;
            if (dir.Equals(Vector3.zero))
            {
                dir = transform.forward;
            }
            else
            {
                dir.Normalize();
            }

            targetVelocity = speed * dir;


            Vector3 velocityChange = targetVelocity - currVelocity;
            velocityChange.y = 0;
            myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        //apply gravity
        myRigidbody.AddForce(new Vector3(0, -gravity * myRigidbody.mass, 0));
        Vector3 currentVelocity = myRigidbody.velocity;
        if (currentVelocity.y > maxYVelocity)
        {
            currentVelocity = new Vector3(currentVelocity.x, maxYVelocity, currentVelocity.z);
            myRigidbody.velocity = currentVelocity;
        }
    }

    Vector3 Rotating(float horizontal, float vertical, bool ignoreCameraRot = false)
    {
        //Debug.Log("Rotating " + horizontal +"," + vertical + "," + ignoreCameraRot);
        Vector3 forward;
        if (cameraTransform != null)
        {
            forward = cameraTransform.TransformDirection(Vector3.forward);
        }
        else
        {
            forward = GameManager.instance.ActiveCamera.transform.TransformDirection(Vector3.up);
        }
        forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;

        float finalTurnSmoothing = turnSmoothing;

        targetDirection = forward* vertical + right* horizontal;

        if ((isMoving && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            float turnAngle = Quaternion.Angle(targetRotation, myRigidbody.rotation);
            Quaternion newRotation = Quaternion.Slerp(myRigidbody.rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
            myRigidbody.MoveRotation(newRotation);
            lastDirection = targetDirection;

            anim.SetFloat("Turn", turnAngle, 0.1f, Time.deltaTime);
        }   
        
        return targetDirection;
    }

    private void Repositioning(float repositionTurnSmoothing)
    {
        Vector3 repositioning = lastDirection;
        if (repositioning != Vector3.zero)
        {
            repositioning.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(repositioning, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, repositionTurnSmoothing * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

    public bool isRunning()
    {
        return run;
    }

    public void stopMoving()
    {
        speed = 0f;
        isMoving = false;        
    }    
        
    public float Speed
    {
        get { return speed; }
    }

    public void setAimRight()
    {
        rightAim = true;
    }

    public void setAimLeft()
    {
        rightAim = false;
    }

    public void toggleAimSide()
    {
        rightAim = !rightAim;
    }

    public bool isAimingRight()
    {
        return rightAim && aim;
    }
    

    public void lockControl()
    {
        myRigidbody.isKinematic = true;
        enabled = false;
    }

    public void unlockControl()
    {
        myRigidbody.isKinematic = false;
        enabled = true;
    }    
}
