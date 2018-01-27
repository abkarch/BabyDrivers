﻿using UnityEngine;
using System;

public class BabyCarController : MonoBehaviour
{
    public static BabyCarController instance;
    public Rigidbody carRigidbody;

    [Tooltip("Should point to a steering wheel for this car. Will be used for turning.")]
    public SteeringWheel steeringWheel;
    [Tooltip("Should point to the pedal for the gas.")]
    public CarPedal gasPedal;
    [Tooltip("Should point to the pedal for the brakes.")]
    public CarPedal brakePedal;
    

    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 30f;
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;

    private WheelCollider[] m_Wheels;

    void Awake()
    {
        instance = this;
        if (carRigidbody == null)
        {
            GetComponent<Rigidbody>();
        }
    }

    // Find all the WheelColliders down in the hierarchy.
    void Start()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    // This is a really simple approach to updating wheels.
    // We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
    // This helps us to figure our which wheels are front ones and which are rear.
    void Update()
    {
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        float angle = GetTurningAngle();
        float torque = GetTorqueValue();

        float handBrake = GetBrakingValue();

        foreach (WheelCollider wheel in m_Wheels)
        {
            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;

            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            // Update visual wheels if any.
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }
        }
    }

    public float GetBrakingValue()
    {
        // Get value from brake pedal
        if (brakePedal != null)
        {
            return brakeTorque * brakePedal.PedalValue;
        }
        if (Input.GetKey(KeyCode.X))
        {
            return brakeTorque;
        }
        else
        {
            return 0;
        }
    }

    public float GetTorqueValue()
    {
        // Get value from gas pedal
        if (gasPedal != null)
        {
            return maxTorque * gasPedal.PedalValue;
        }
        return maxTorque* Input.GetAxis("VerticalP1");
    }

    public float GetTurningAngle()
    {
        //get turning angle from steering wheel
        if (steeringWheel != null)
        {
            return steeringWheel.GetSteeringWheelAngle();
        }
        return maxAngle * Input.GetAxis("HorizontalP1");
    }
}