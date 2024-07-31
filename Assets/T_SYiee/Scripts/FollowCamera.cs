using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[Header("Target Player")]

	public Transform Target;
	public float Target_y;
	public Transform DropKillChecking;

	[Header("Shaking")]
	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.1f;
	public float decreaseFactor = 1.0f;


	private Transform _camTransform;
	private float _followSpeed = 5f;

	Vector3 originalPos;

	void Awake()
	{
		Cursor.visible = false;
		if (_camTransform == null)
		{
			_camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = _camTransform.localPosition;
	}

	private void Update()
	{

		if (_camTransform.localPosition.y <= DropKillChecking.position.y)
		{
			return;
		}

		Vector3 newPosition = Target.position;
		newPosition.z = -8;
		newPosition.y = newPosition.y + Target_y;
		transform.position = Vector3.Slerp(transform.position, newPosition, _followSpeed * Time.deltaTime);

		originalPos = transform.position;

		if (shakeDuration > 0)
		{
			_camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}

	}

	public void ShakeCamera()
	{
		originalPos = _camTransform.localPosition;
		shakeDuration = 0.5f;
	}
}
