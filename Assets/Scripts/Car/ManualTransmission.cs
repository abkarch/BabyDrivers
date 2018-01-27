
using UnityEngine;
using System;

public class ManualTransmission : MonoBehaviour
{

    public CarPedal clutchPedal;
    
    public void SwitchGears()
    {
        if(clutchPedal.PedalValue==1)
        {
            //Gear shifter is interactable
        }
    }



}