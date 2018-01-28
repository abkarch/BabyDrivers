using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNames : MonoBehaviour {

    // Use this for initialization
    public Transform namePlate1;
    public Transform namePlate2;
    public Transform namePlate3;
    public Transform namePlate4;

    public Transform player1Cam;
    public Transform player2Cam;
    public Transform player3Cam;
    public Transform player4Cam;

    void Start()
    {

    }

    void Update ()
    {
        namePlate1.LookAt(player1Cam);
        namePlate2.LookAt(player2Cam);
        namePlate3.LookAt(player3Cam);
        namePlate4.LookAt(player4Cam);
	}
}
