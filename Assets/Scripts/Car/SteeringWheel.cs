using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringWheel : MonoBehaviour
{
    [Tooltip("How much can the wheel be rotated in each direction")]
    public float MaxAngle = 180;
    [Tooltip("How many degrees per second does the wheel rotate back to zero if left alone")]
    public float resetRotationRate;

    float angle = 0;

    void Update()
    {
        //TODO: get rid of this, since we'll use code to Rotate from Baby Interaction with the wheel
        //I'm just testing by rotating in the editor
        MatchAngleToTransform();
    }

    //TODO: call this while baby isn't interacting with the wheel
    public void RotateWheelBackToZero()
    {
        if (angle == 0) return;
        if (angle < 0)
        {
            Rotate(resetRotationRate * Time.deltaTime);
        }
        else
        {
            Rotate(-resetRotationRate * Time.deltaTime);
        }
    }


    public float GetSteeringWheelAngle()
    {
        return angle;
    }

    public void Rotate(float delataAngle)
    {
        angle += delataAngle;
        angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle);
        MatchTransformToAngle();
    }
    
    public void MatchTransformToAngle()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
    }

    public void MatchAngleToTransform()
    {
        angle = transform.localEulerAngles.z;
    }
}