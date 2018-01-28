using System;
using System.Collections;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public int playerNum = 1;
    public PhysicsPlayerController playerController;
    public Rigidbody objRigidbody;
    public string state;
    public Animator anim;
    public BabyCarController car;
    public bool canShiftChange;
	public AudioSource Audio;

    triggerZone zoneIn = null;
    triggerZone zoneActingIn = null;

    private void Start()
    {
        state = "free";
        canShiftChange = true;
        car = BabyCarController.instance;

        if (objRigidbody != null)
        {
            objRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        if (playerController == null)
        {
            playerController = GetComponent<PhysicsPlayerController>();
        }
        if (playerController != null)
        {
            playerController.SetPlayerNum(playerNum);
        }
    }

    private void Update()
    {
        if (state == "free")
        {
            if (zoneIn != null)
            {
                if (Input.GetButtonDown("EnterPositionP" + playerNum))
                {
                    zoneIn.StartUsing(this);
                }
            }
        }
        else if (state == "leavingInteraction")
        { // nothing to do - it'll wait until the Ienumerator is done

        }
        else
        {
            if (Input.GetButtonDown("LeavePositionP" + playerNum))
            {
                if (zoneActingIn != null)
                {
                    zoneActingIn.LeaveState();
                }
            }
            else if (state == "Steering")
            {
                RunSteeringState();
            } else if (state == "Pedaling") {
                RunPedalingState();
            } else if (state == "Shifting") {
                RunShiftingState();
            }
        }
    }

    public void SetPlayerNum(int num)
    {
        playerNum = num;
        if (playerController)
        {
            playerController.SetPlayerNum(num);
        }
        playerController.SetPlayerNum(num);
    }

    public void setState(string s, triggerZone t)
    {
        if (s == state)
        {
            return;
        }
        else if (s == "free")
        {
            enablePhysics();
            anim.SetBool("Steering", false);
            anim.SetBool("Pedaling", false);
            state = s;
            zoneActingIn = null;            
        }
        else if (s == "leavingInteraction")
        {
            anim.SetBool("Steering", false);
            anim.SetBool("Pedaling", false);
            state = s;
            StartCoroutine(tweenBabyToFreeState(gameObject, zoneActingIn.exitToPosition, 15));
        }
        else if (t != null)
        {
            disablePhysics();
            state = s;
            gameObject.transform.parent = car.gameObject.transform;
            StartCoroutine(tweenBaby(this.gameObject, t.babyLocation, 15));
            
            Debug.Log("Setting bool: " + s);
            anim.SetBool(s, true);
            zoneActingIn = t;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        triggerZone t = other.GetComponent<triggerZone>();
        if (t != null)
        {
            zoneIn = t;
        }
    }

    void OnTriggerExit(Collider other)
    {
        triggerZone t = other.GetComponent<triggerZone>();
        if (t == zoneIn)
        {
            zoneIn = null;
        }
    }

    public void disablePhysics()
    {
        if (playerController != null)
        {
            playerController.lockControl();
        }
        objRigidbody.isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void enablePhysics()
    {
        if (playerController != null)
        {
            playerController.unlockControl();
        }
        objRigidbody.isKinematic = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private IEnumerator tweenBaby(GameObject g, Transform newPos, float rate)
    {
        while (!(g.transform.position.AlmostEquals(newPos.transform.position, .05f)) || g.transform.rotation != (newPos.transform.rotation))
        {
            g.transform.position = Vector3.Lerp(g.transform.position, newPos.transform.position, Time.deltaTime * rate);
            g.transform.rotation = Quaternion.Slerp(g.transform.rotation, newPos.transform.rotation, Time.deltaTime * rate);
            yield return null;
        };
    }

    private IEnumerator tweenBabyToFreeState(GameObject g, Transform newPos, float rate)
    {
        while (!(g.transform.position.AlmostEquals(newPos.transform.position, .05f)) || g.transform.rotation != (newPos.transform.rotation))
        {
            g.transform.position = Vector3.Lerp(g.transform.position, newPos.transform.position, Time.deltaTime * rate);
            g.transform.rotation = Quaternion.Slerp(g.transform.rotation, newPos.transform.rotation, Time.deltaTime * rate);
            yield return null;
        };
        setState("free", null);
    }

    public void RunSteeringState()
    {

        float turn = Input.GetAxis("HorizontalP" + playerNum);
        anim.SetFloat("Turn", 01.5f * -turn, 0.1f, Time.deltaTime);
        if (turn < 0)
        {
            car.steeringWheel.Rotate(-Mathf.Sign(turn) * car.steeringWheel.rate * Time.deltaTime);
            this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, car.steeringWheel.leftRotationPlayerPos.position, 10 * Time.deltaTime);
        }
        else if (turn > 0)
        {
            car.steeringWheel.Rotate(-Mathf.Sign(turn) * car.steeringWheel.rate * Time.deltaTime);
            this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, car.steeringWheel.rightRotationPlayerPos.position, 10 * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, car.steeringWheel.originalPlayerPos.position, 10 * Time.deltaTime);
            car.steeringWheel.RotateWheelBackToZero();
        }

    }

    public void RunPedalingState() {
        float gasIn = Input.GetAxis("Interact1P" + playerNum);
        if (gasIn == 0 && Input.GetButton("RightBumper" + playerNum))
        {
            gasIn = 1;
        }
        Debug.Log(gasIn);
        float brakeIn = Input.GetAxis("Interact2P" + playerNum);
        if (brakeIn == 0 && Input.GetButton("LeftBumper" + playerNum))
        {
            brakeIn = 1;
        }
        anim.SetFloat("gas", Mathf.Lerp(anim.GetFloat("gas"), gasIn, 5 * Time.deltaTime));
        anim.SetFloat("brake", Mathf.Lerp(anim.GetFloat("brake"), brakeIn, 5 * Time.deltaTime));
        anim.SetFloat("idle", 1.0f);

        car.gasPedal.PedalValue = gasIn;
        car.brakePedal.PedalValue = brakeIn;
    }

    public void RunShiftingState() {
        float h = Input.GetAxis("HorizontalP" + playerNum);
        if (h < -0.5f && canShiftChange) {
            if (car.gearShift.ShiftGearUp()) {
                StartCoroutine("animTimer");
                anim.CrossFade("swipeLeft", 0.1f);
            }
        } else if (h > 0.5f && canShiftChange) {
            if (car.gearShift.ShiftGearDown()) {
                StartCoroutine("animTimer");
                anim.CrossFade("swipeRight", 0.1f);
            }
        }
    }

    public IEnumerator animTimer() {
        canShiftChange = false;
        yield return new WaitForSeconds(2.0f);
        canShiftChange = true;
    }

	public void PlaySoundClip(AudioClip inSound)
	{
		if (Audio != null && inSound != null)
		{
			Audio.clip = inSound;
			Audio.Play();
		}
	}
}

