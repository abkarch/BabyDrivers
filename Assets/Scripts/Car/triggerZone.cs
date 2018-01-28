using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerZone : MonoBehaviour {

    public Transform babyLocation;
    public Transform exitToPosition;
    public string newStateOfBaby;
    public bool occupied;
    Baby babyUsing = null;

	public AudioClip EnterSound;
	public AudioClip LeaveSound;

    public void StartUsing(Baby b)
    {
        if (occupied) return;
        babyUsing = b;
        occupied = false;
        babyUsing.setState(newStateOfBaby, this);

		babyUsing.PlaySoundClip(EnterSound);
    }

<<<<<<< HEAD
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

		babyUsing.PlaySoundClip(LeaveSound);

        babyUsing = null;
        occupied = false;
=======
    public void LeaveState()
    {
        if (babyUsing != null)
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
>>>>>>> 6ae6734b4994e1c8b48b7eb95b09039d2e4613c3
    }
}
