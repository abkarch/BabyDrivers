using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GearShift : MonoBehaviour
{
    public enum Gear
    {
        Park,
        Reverse,
        Drive
    }

    [Tooltip("Gear types in order for this gear shift.")]
    public Gear[] possibleGears;
    public Transform[] IndicatorTransitionLocations;
    public GameObject gearIndicator;
    public Transform gearShaft;
    private Vector3[] rots;
    public int currentGearIndex = 0;

    public Gear CurrentGear
    {
        get { return possibleGears[currentGearIndex]; }
        set
        {
            for (int i = 0; i < possibleGears.Length; i++)
            {
                if (possibleGears[i] == value)
                {
                    currentGearIndex = i;
                    UpdateVisual(true);
                    break;
                }
            }
        }
    }

    void Awake()
    {
        //make sure there are possible gears
        if (possibleGears == null || possibleGears.Length < 1)
        {
            possibleGears = new Gear[3];
            possibleGears[0] = Gear.Park;
            possibleGears[1] = Gear.Reverse;
            possibleGears[2] = Gear.Drive;
        }


    }

    public void Start() {
        rots = new Vector3[3];
        rots[0] = new Vector3(0, 30, 0);
        rots[1] = new Vector3(0, 0, 0);
        rots[2] = new Vector3(0, -30, 0);
    }

    public void UpdateVisual(bool b)
    {
        gearIndicator.transform.position = IndicatorTransitionLocations[currentGearIndex].transform.position;
        if(b)
          gearShaft.transform.Rotate(rots[0]);
        else
          gearShaft.transform.Rotate(rots[2]);
    }

    public bool ShiftGearUp()
    { // shifting up goes down in the list
        if (currentGearIndex > 0)
        {
            currentGearIndex--;
            UpdateVisual(true);
            return true;
        }
        return false;
    }

    public bool ShiftGearDown()
    {
        //shifting the gear down goes up in the list
        if (currentGearIndex < possibleGears.Length - 1)
        {
            currentGearIndex++;
            UpdateVisual(false);
            return true;
        }
        return false;
    }
}