using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPedal : MonoBehaviour
{
    public Transform depthMarker;
    //pedal value represents a percentage pedal down between 0 and 1
    float pedalValue = 0;
    // TODO: Pedal value will be set from baby interaction with the pedals
    public float PedalValue { 
        get {
            return pedalValue;
        } set {
            pedalValue = Mathf.Clamp01(value);
            RefreshVisuals();
        }
    }
    
    public void RefreshVisuals()
    {
        transform.position += new Vector3(depthMarker.position.x * pedalValue, 0, 0);
    }    
}