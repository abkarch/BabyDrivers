using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerZone : MonoBehaviour {

    public Transform babyLocation;
    public Transform exitToPosition;
    public string newStateOfBaby;
    public bool occupied;
    Baby babyUsing = null;

    public void StartUsing(Baby b)
    {
        if (occupied) return;
        babyUsing = b;
        occupied = false;
        babyUsing.setState(newStateOfBaby, this);
    }

    public void LeaveState()
    {
        if (exitToPosition != null)
        {
            babyUsing.setState("leavingInteraction", this);
        }
        else
        {
            babyUsing.setState("free", null);
        }
        babyUsing = null;
        occupied = false;
    }
}
