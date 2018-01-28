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
                    UpdateVisual();
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

    public void UpdateVisual()
    {
        //TODO: move the gear visually to the currentGear
    }

    public bool ShiftGearUp()
    { // shifting up goes down in the list
        if (currentGearIndex > 0)
        {
            currentGearIndex--;
            UpdateVisual();
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
            UpdateVisual();
            return true;
        }
        return false;
    }
}