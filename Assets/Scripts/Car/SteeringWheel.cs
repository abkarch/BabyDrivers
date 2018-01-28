using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringWheel : MonoBehaviour
{
    [Tooltip("How much can the wheel be rotated in each direction")]
    public float MaxAngle = 45;
    [Tooltip("How many degrees per second does the wheel rotate back to zero if left alone")]
    public float resetRotationRate;
    public Transform leftRotationPlayerPos;
    public Transform rightRotationPlayerPos;
    public Transform originalPlayerPos;

    float angle = 0;
    public float rate;
    void Update()
    {
        //TODO: get rid of this, since we'll use code to Rotate from Baby Interaction with the wheel
        //I'm just testing by rotating in the editor
        //MatchAngleToTransform();
    }

    //TODO: call this while baby isn't interacting with the wheel
    public void RotateWheelBackToZero()
    {
        MatchAngleToTransform();
        float rotationChange = resetRotationRate * Time.deltaTime;
        if (angle == 0) return;
        if (angle < 0) {
            Rotate(resetRotationRate * Time.deltaTime);
            if (angle > 0) {
                angle = 0;
                MatchTransformToAngle();
            }
        } else {
            Rotate(-resetRotationRate * Time.deltaTime);
            if (angle > 0) {
                angle = 0;
                MatchTransformToAngle();
            }                   
        }
    }


    public float GetSteeringWheelAngle()
    {
        return -angle;
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