using UnityEngine;
using System.Collections;

public class PhysicsPlayerController : MonoBehaviour
{
    public int playerNum = 1;
    public Rigidbody myRigidbody;
    public float gravity = 9.8f;
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

	public float camSpeed = 4f;
    private Transform cameraTransform;
	private ThirdPersonCamera tpCam;

    private float h;
    private float v;
    private bool wantsToJump = false;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    private float distToGround = 0;

    private bool isGrounded = false;

    private bool canMove = true;

	public void Initialize(Transform inCamTran)
	{
		cameraTransform = inCamTran;
		tpCam = cameraTransform.GetComponent<ThirdPersonCamera>();
	}

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
    }

    public void SetPlayerNum(int num)
    {
        playerNum = num;
    }

    bool CalculateIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position +new Vector3(0,.01f, 0), -Vector3.up, distToGround + 0.2f);
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
        if (!wantsToJump)
        {
            wantsToJump = Input.GetButtonDown("JumpP" + playerNum);
        }

        isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

		if (tpCam != null)
		{
			float viewX = Input.GetAxis("ViewXP" + playerNum);
			float viewY = Input.GetAxis("ViewYP" + playerNum);

			if (viewX == 0 && viewY == 0)
			{
				// This is intended for debugging.
				viewX = Input.GetAxis("MouseX");
				viewY = Input.GetAxis("MouseY");
			}
			else
			{
				float camPace = camSpeed * Time.deltaTime;
				viewX *= camPace;
				viewY *= camPace;
			}

			tpCam.UpdateFromInput(viewX, viewY);
		}
    }

    void FixedUpdate()
    {
        if (!canMove) return;        
        MovementManagement(h, v);

        JumpManagement();
    }

    void JumpManagement()
    {
        if (myRigidbody.velocity.y < 10) // already jumped
        {
            if (timeToNextJump > 0)
                timeToNextJump -= Time.deltaTime;
        }
        CalculateIsGrounded();
        if (isGrounded)
        {
            if (wantsToJump)
            {
                if (timeToNextJump <= 0)
                {
                    myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpVelocity, myRigidbody.velocity.z);
                    timeToNextJump = jumpCooldown;
                    isGrounded = false;
                }
            }
        }
        if (!isGrounded)
        {
            anim.SetFloat("Jump", myRigidbody.velocity.y);
        }
        else
        {
            anim.SetFloat("Jump", 0);
        }
        anim.SetBool("OnGround", isGrounded);
        wantsToJump = false;
    }

    void MovementManagement(float horizontal, float vertical)
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
				forward = cameraTransform.forward;// cameraTransform.TransformDirection(Vector3.up);
            }
            else
            {
				Debug.LogWarning("cameraTransform was missing when setting up forward vector!");
                if (GameManager.instance != null)
                {
                    forward = GameManager.instance.ActiveCamera.transform.TransformDirection(Vector3.up);
                }
                else
                {
                    forward = Camera.main.transform.TransformDirection(Vector3.up);
                }
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
            if (GameManager.instance != null)
            {
                forward = GameManager.instance.ActiveCamera.transform.TransformDirection(Vector3.up);
            }
            else
            {
                forward = Camera.main.transform.TransformDirection(Vector3.up);

            }
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

    public void stopMoving()
    {
        speed = 0f;
        isMoving = false;        
    }    
        
    public float Speed
    {
        get { return speed; }
    }

    public void lockControl()
    {
        Debug.Log("lock control");
        myRigidbody.isKinematic = true;
        canMove = false;
    }

    public void unlockControl()
    {
        Debug.Log("unlock control");
        myRigidbody.isKinematic = false;
        canMove = true;
    }    
}
