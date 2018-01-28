using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
	public float MinDistance = 0.3f;
	public float Distance = 3f;
	public float MaxVerticalAngle = 75f;
	public LayerMask ColliderLayer;

	private Transform _Target = null;
	private Camera _Cam = null;
	public Camera Cam
	{
		get { return _Cam; }
	}

	private float _CurrentX = 0f;
	private float _CurrentY = 0f;

	private Vector3 _Offset = Vector3.up * 0.3f;

	public void Initialize(GameObject inTarget)
	{
		_Target = inTarget.transform;

		_Cam = GetComponent<Camera>();

		Debug.LogFormat("Did we find target and cam? {0} {1}", _Target, _Cam);
	}

	public void UpdateFromInput(float inX, float inY)
	{
		Vector3 origin = _Target.position + _Offset;
		RaycastHit hit;

		// X will wrap
		float nextX = _CurrentX + inX;
		if (nextX > 180f) nextX -= 360f;
		if (nextX < -180f) nextX += 360f;

		Vector3 toCam = Quaternion.Euler(_CurrentY, nextX, 0f) * Vector3.forward;
		if (Physics.SphereCast(origin, Cam.nearClipPlane, toCam, out hit, Distance, ColliderLayer) == true)
		{
			if (hit.distance > MinDistance)
			{
				_CurrentX = nextX;
			}
		}
		else
		{
			_CurrentX = nextX;
		}

		// Y is clamped
		float nextY = _CurrentY + inY;
		if (nextY > MaxVerticalAngle) nextY = MaxVerticalAngle;
		if (nextY < -MaxVerticalAngle) nextY = -MaxVerticalAngle;

		toCam = Quaternion.Euler(nextY, _CurrentX, 0f) * Vector3.forward;
		if (Physics.SphereCast(origin, Cam.nearClipPlane, toCam, out hit, Distance, ColliderLayer) == true)
		{
			if (hit.distance > MinDistance)
			{
				_CurrentY = nextY;
			}
		}
		else
		{
			_CurrentY = nextY;
		}
	}

	private void LateUpdate()
	{
		// Don't continue if we don't have our target yet.
		if (_Target == null || _Cam == null)
		{
			return;
		}
			
		Vector3 toCam = Quaternion.Euler(_CurrentY, _CurrentX, 0f) * Vector3.forward;

		Vector3 origin = _Target.position + _Offset;
		RaycastHit hit;
		if (Physics.SphereCast(origin, Cam.nearClipPlane, toCam, out hit, Distance, ColliderLayer) == true)
		{
			transform.position = hit.point + (hit.normal * _Cam.nearClipPlane);
		}
		else
		{
			transform.position = origin + (toCam * Distance);
		}

		transform.rotation = Quaternion.LookRotation(origin - transform.position, Vector3.up);
	}
}
