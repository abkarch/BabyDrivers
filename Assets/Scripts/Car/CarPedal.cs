using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPedal : MonoBehaviour
{
    //pedal value represents a percentage pedal down between 0 and 1
    float pedalValue = 0;
    // TODO: Pedal value will be set from baby interaction with the pedals
    public float PedalValue
    {
        get
        {
            return pedalValue;
        }
        set
        {
            pedalValue = Mathf.Clamp01(value);
            RefreshVisuals();
        }
    }
    
    public void RefreshVisuals()
    {
        //TODO: set the pedal to look like it is pushed down appropriately
        //maybe use an animation

    }    
}