using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour {

    private Vector3 destinationPosition;
    private GameObject body;
    private Vector3 thisPosition;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
        body = GameObject.Find("Capsule");
        destinationPosition = body.transform.position;
        //body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        thisPosition = transform.position;
        direction = transform.InverseTransformPoint(destinationPosition);
        float s = Mathf.Atan2(direction.x, direction.z);
        transform.Rotate(new Vector3(s, 0, 0));
    }
}
