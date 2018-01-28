using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNames : MonoBehaviour {

    // Use this for initialization
    public Transform namePlate1;
    public Transform namePlate2;
    public Transform namePlate3;
    public Transform namePlate4;

    public static Transform player1Cam;
    public static Transform player2Cam;
    public static Transform player3Cam;
    public static Transform player4Cam;

    void Start()
    {
        namePlate1.GetComponent<TextMesh>().text = PlayerNamesData.names[0];
        namePlate2.GetComponent<TextMesh>().text = PlayerNamesData.names[1];
        namePlate3.GetComponent<TextMesh>().text = PlayerNamesData.names[2];
        namePlate4.GetComponent<TextMesh>().text = PlayerNamesData.names[3];

        //assign player cams

        
    }

    void Update ()
    {
        namePlate1.LookAt(player1Cam);
        namePlate2.LookAt(player2Cam);
        namePlate3.LookAt(player3Cam);
        namePlate4.LookAt(player4Cam);
	}
}
