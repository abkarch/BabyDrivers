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
    private float velocity;
    private float height;
    private bool collision;
    private GameObject car;

    // Use this for initialization
    void Start () {
        collision = false;
        health = MAX_HEALTH;
        speed = 0.0F;
        velocity = 0.0F;
        car = GameObject.Find("Car");
	}
	
	// Update is called once per frame
	void Update () {

        speed = Mathf.Abs(velocity);

        // Car dies
        if (health <= 0 || height < MINIMUM_HEIGHT)
        {
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
