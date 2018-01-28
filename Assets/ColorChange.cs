using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{

    public Material[] colors = new Material[9];
    public GameObject[] bodyParts1 = new GameObject[6];
    public GameObject[] bodyParts2 = new GameObject[6];
    public GameObject[] bodyParts3 = new GameObject[6];
    public GameObject[] bodyParts4 = new GameObject[6];

    
    public int[] colorIndexes = new int[4] { 0, 0, 0, 0 };

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("RightBumper1"))//change this when find out actual color
        {
            Debug.Log("Changing color");
            ResetIndex(0);
            LoopBodyParts(0, bodyParts1);
        }

        if (Input.GetButtonDown("RightBumper2"))//change this when find out actual color
        {
            ResetIndex(1);
            LoopBodyParts(1, bodyParts2);
        }
        if (Input.GetButtonDown("RightBumper3"))//change this when find out actual color
        {
            ResetIndex(2);
            LoopBodyParts(2, bodyParts3);
        }
        if (Input.GetButtonDown("RightBumper4"))//change this when find out actual color
        {
            ResetIndex(3);
            LoopBodyParts(3, bodyParts4);
        }
        if (Input.GetButtonDown("LeftBumper1"))//change this when find out actual color
        {
            ResetNegativeIndex(0);
            LoopBodyParts(0, bodyParts1);
        }
        if (Input.GetButtonDown("LeftBumper2"))//change this when find out actual color
        {
            ResetNegativeIndex(1);
            LoopBodyParts(1, bodyParts2);
        }
        if (Input.GetButtonDown("LeftBumper3"))//change this when find out actual color
        {
            ResetNegativeIndex(2);
            LoopBodyParts(2, bodyParts3);
        }
        if (Input.GetButtonDown("LeftBumper4"))//change this when find out actual color
        {
            ResetNegativeIndex(3);
            LoopBodyParts(3, bodyParts4);
        }

    }
    private void LoopBodyParts(int playerNum, GameObject[] bodyParts)
    {
        foreach (GameObject body in bodyParts)
        {
            body.GetComponent<SkinnedMeshRenderer>().materials = new Material[] { colors[colorIndexes[playerNum]] };
            Debug.Log("colors[colorIndexes[playerNum]]");
        }
    }

    private void ResetIndex(int index)
    {
        if (colorIndexes[index] == 8)
        {
            colorIndexes[index] = 0;
        }
        else
        {
            colorIndexes[index]++;
        }
    }
    private void ResetNegativeIndex(int index)
    {
        if (colorIndexes[index] == 0)
        {
            colorIndexes[index] = 8;
        }
        else
        {
            colorIndexes[index]--;
        }
    }

}