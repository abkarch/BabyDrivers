using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    public int MAX_HEALTH = 1000;
    public  int HIGH_DAMAGE = 200;
    public  int MEDIUM_DAMAGE = 100;
    public  int LOW_DAMAGE = 10;
    public  int LOW_HEALTH_THRESHOLD = 300;
    public  float HIGH_SPEED = 10.0F;
    public  float MEDIUM_SPEED = 5.0F;
    public  float MINIMUM_HEIGHT = -10.0F;

    public PlayerManager playerManager;
    public Camera crashCamera;
     bool carAlive=true;

    public int health;
    private float speed;
    private float height;
    public Rigidbody body;

    public GameObject DamageEffects;

    // Use this for initialization
    void Start () {
        health = MAX_HEALTH;
        speed = 0.0F;
        body = this.gameObject.GetComponentInChildren<Rigidbody>();
        height = body.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        
        speed = body.velocity.magnitude;
        height = body.position.y;

        if (carAlive)
        {
            if (health <= LOW_HEALTH_THRESHOLD)
            {
                DamageEffects.GetComponent<Explosions>().LowHealthFire();
            }

            // Car dies
            if (health <= 0 || height < MINIMUM_HEIGHT)
            {
                
                crashCamera.enabled = true;
                playerManager.gameObject.GetComponentInChildren<SplitScreen>().cam1.enabled = false;
                playerManager.gameObject.GetComponentInChildren<SplitScreen>().setCam(1, crashCamera);
                DamageEffects.GetComponent<Explosions>().SetOffExplosions();

                
                Restart();
            }
        }
    }

    // On collision, subtract from health depending on speed.
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer .Equals("Baby"))
            return;
        
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
    
    void Restart()
    {
        carAlive = false;
        if(Input.GetButtonDown("EnterPositionP1"))
        {
            Application.LoadLevel("StartScren");
        }
    }
}
