using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    private const int MAX_HEALTH = 1000;
    private const int HIGH_DAMAGE = 200;
    private const int MEDIUM_DAMAGE = 100;
    private const int LOW_DAMAGE = 10;
    private const float HIGH_SPEED = 10.0F;
    private const float MEDIUM_SPEED = 5.0F;
    private const float MINIMUM_HEIGHT = -10.0F;

    private int health;
    private float speed;
    private float height;
    private Rigidbody body;

    // Use this for initialization
    void Start () {
        health = MAX_HEALTH;
        speed = 0.0F;
        body = BabyCarController.instance.carRigidbody;
        height = body.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        speed = body.velocity.magnitude;
        height = body.position.y;

        // Car dies
        if (health <= 0 || height < MINIMUM_HEIGHT)
        {
            body.position = new Vector3(0,0,0);
            Start();
        }
    }

    // On collision, subtract from health depending on speed.
    void OnCollisionEnter(Collision col)
    {
        if (speed >= HIGH_SPEED)
        {
            health -= HIGH_DAMAGE;
        }
        else if (speed >= MEDIUM_SPEED)
        {
            health -= MEDIUM_DAMAGE;
        }
        else
        {
            health -= LOW_DAMAGE;
        }
    }
}
