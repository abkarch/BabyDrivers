using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPedal : MonoBehaviour
{
    public Transform depthMarker;
    public Transform originalPosition;
    public float pedalResetRate;
    public float pedalPushRate;

    //pedal value represents a percentage pedal down between 0 and 1
    float pedalValue = 0;

    // TODO: Pedal value will be set from baby interaction with the pedals
    public float PedalValue { 
        get {
            return pedalValue;
        } set {
            if (value == 0)
            {
                pedalValue = 0;
                resetPedal();
            }
            else {
                pedalValue = Mathf.Clamp01(value);
                pushPedal();
            }
        }
    }
    
    public void pushPedal()
    {
        Debug.Log("Pedal value: " + PedalValue);
        Vector3 t = depthMarker.transform.position - gameObject.transform.position;
        t *= PedalValue;
        transform.position = Vector3.Lerp(gameObject.transform.position, depthMarker.transform.position, Time.deltaTime * pedalPushRate);
    }    

    public void resetPedal() {
        transform.position = Vector3.Lerp(gameObject.transform.position, originalPosition.position, Time.deltaTime * pedalResetRate);
    }
}