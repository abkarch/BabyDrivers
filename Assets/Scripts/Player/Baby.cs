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

    private void Start()
    {
        state = "free";

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

    private void FixedUpdate()
    {
        if (state == "Steering")
        {
            RunSteeringState();
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
        }
        else if (t != null)
        {
            disablePhysics();
            StartCoroutine(tweenBaby(this.gameObject, t.babyLocation));
            anim.SetBool(s, true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        triggerZone t = null;
        if ((t = other.GetComponent<triggerZone>()) != null && state == "free" && Input.GetButtonDown("EnterPositionP" + playerNum))
        {
            setState(t.newStateOfBaby, t);
        }
        else if (state != "free" && Input.GetButtonDown("LeavePositionP" + playerNum))
        {
            setState("free", null);
        }
    }

    public void disablePhysics()
    {
        if (playerController != null)
        {
            playerController.lockControl();
        }
        objRigidbody.isKinematic = false;
        objRigidbody.useGravity = false;
    }

    public void enablePhysics()
    {
        if (playerController != null)
        {
            playerController.unlockControl();
        }
        objRigidbody.isKinematic = true;
        objRigidbody.useGravity = true;
    }

    private IEnumerator tweenBaby(GameObject g, Transform newPos)
    {
        while (g.transform.position != newPos.transform.position && g.transform.rotation != newPos.transform.rotation)
        {
            g.transform.position = Vector3.Lerp(g.transform.position, newPos.transform.position, Time.deltaTime);
            g.transform.Rotate(Vector3.Lerp(g.transform.rotation.eulerAngles, newPos.transform.rotation.eulerAngles, Time.deltaTime));
            yield return null;
        }
    }

    public void RunSteeringState()
    {
        gameObject.transform.parent = car.gameObject.transform;
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
}

