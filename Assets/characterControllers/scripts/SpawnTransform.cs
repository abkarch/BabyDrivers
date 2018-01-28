using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransform : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 0.15f);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
