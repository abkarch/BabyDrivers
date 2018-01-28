using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

    public class Goal
    {
        // Does the car need to be parked on the goal?
        private bool park;
        // Does the car need to collide with the goal?
        private bool collide;
        // Does a non-car object need to collide with the goal?
        private bool deliver;
        // The non-car object that needs to collide with the goal
        private Rigidbody deliveredObject;
        // Does the car need to destroy the goal?
        private bool destroy;
        // Amount of health the goal has
        private int health;
        // Minimum speed for collision to cause damage to goal
        private float minDmgSpeed;
        // Damage dealt to goal when collision is above minimum speed
        private int damage;
        // Does the goal need to be completed in a certian timeframe?
        private bool time;
        // length for timer to count down
        private float timerLength;
        // Whether the goal has ended or not
        private bool goalEnd;
        // body of the car
        private Rigidbody carBody;
        
        public Goal()
        {
            time = false;
            goalEnd = false;
            carBody = BabyCarController.instance.carRigidbody;
        }

        // The goal is to park.
        public void setPark()
        {
            park = true;
            collide = false;
            deliver = false;
            deliveredObject = null;
            destroy = false;
            health = 0;
            time = false;
            timerLength = 0;
        }

        // the goal is to collide
        public void setCollide()
        {
            park = false;
            collide = true;
            deliver = false;
            deliveredObject = null;
            destroy = false;
            health = 0;
            time = false;
            timerLength = 0;
        }

        // the goal is to deliver
        public void setDeliver(Rigidbody obj)
        {
            park = false;
            collide = false;
            deliver = true;
            deliveredObject = obj;
            destroy = false;
            health = 0;
            time = false;
            timerLength = 0;
        }

        // the goal is to destroy
        public void setDestroy(int maxHealth, float minDmgSpeed, int damage)
        {
            park = false;
            collide = false;
            deliver = false;
            deliveredObject = null;
            destroy = true;
            health = maxHealth;
            time = false;
            timerLength = 0;
        }

        // The goal must be completed before the time runs out
        public void setTimer(float timerLength)
        {
            time = true;
            this.timerLength = timerLength;
        }

        // Is the goal to park?
        public bool getPark()
        {
            return park;
        }

        //is the goal to collide?
        public bool getCollide()
        {
            return collide;
        }

        // the goal is to deliver
        public bool getDeliver()
        {
            return deliver;
        }

        // the goal is to destroy
        public bool getDestroy()
        {
            return destroy;
        }

        // The goal must be completed before the time runs out
        public bool getTimer()
        {
            return time;
        }

        // complete this goal
        public void completeGoal()
        {
            goalEnd = true;
        }

        // fail this goal
        public void failGoal()
        {
            goalEnd = true;
        }

        // update the timer
        public void UpdateTime(float delta)
        {
            if (time)
            {
                timerLength -= delta;
                if (timerLength <= 0)
                {
                    failGoal();
                }
            }
        }

        // has goal ended?
        public bool ended()
        {
            return goalEnd;
        }

        public Rigidbody getCarBody()
        {
            return carBody;
        }

        public Rigidbody getDeliveredObject()
        {
            return deliveredObject;
        }

        public void Damage(Collision col)
        {
            // if collision is fast enough, cause damage
            if (col.rigidbody.velocity.magnitude > minDmgSpeed)
            {
                health -= damage;
            }
            // goal completes when health runs out
            if (health <= 0)
            {
                completeGoal();
            }
        }

    }

    private int iterator;
    private Goal[] list = new Goal[4];

	// Use this for initialization
	void Start () {
        list[0] = new Goal();
        list[0].setCollide();
        list[1] = new Goal();
        list[1].setPark();
        list[2] = new Goal();
        list[2].setDeliver(list[2].getCarBody());
        list[3] = new Goal();
        list[3].setDestroy(1000, 0.0F, 100);
        iterator = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(list[iterator].ended())
        {
            iterator++;
        }
        if(list[iterator].getTimer())
        {
            list[iterator].UpdateTime(Time.deltaTime);
        }
	}

    void OnCollisionEnter(Collision col)
    {
        Goal goal = list[iterator];
        Rigidbody carBody = goal.getCarBody();
        Rigidbody deliveredObject = goal.getDeliveredObject();
        if (goal.getPark())
        {
            // car must collide on goal while stopped
            if (col.rigidbody == carBody && carBody.velocity.magnitude <= 0.2)
            {
                goal.completeGoal();
            }
        }
        else if (goal.getCollide())
        {
            // car must collide with goal
            if (col.rigidbody == carBody)
            {
                goal.completeGoal();
            }
        }
        else if (goal.getDeliver())
        {
            // object to be delivered must collide with goal
            if (col.rigidbody == deliveredObject)
            {
                goal.completeGoal();
            }
        }
        else if (goal.getDestroy())
        {
            goal.Damage(col);
        }
        else
        {
        }
    }
}
