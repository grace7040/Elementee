using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[Header("Target Player")]

	public Transform Target;
	public float Target_y;
	public Transform DropKillChecking;

	//Shacking
	// How long the object should shake for.
	float _shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	float _shakeAmount = 0.1f;
	float _decreaseFactor = 1.0f;


	private Transform _camTransform;
	private float _followSpeed = 3.5f;

	Vector3 _originalPos;
	Vector3 _newPosition;

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
		_originalPos = _camTransform.localPosition;
	}

	private void Update()
	{

		if (_camTransform.localPosition.y <= DropKillChecking.position.y)
		{
			return;
		}

		_newPosition = Target.position;
		_newPosition.z = -8;
		_newPosition.y = _newPosition.y + Target_y;
		transform.position = Vector3.Slerp(transform.position, _newPosition, _followSpeed * Time.deltaTime);

		_originalPos = transform.position;

		if (_shakeDuration > 0)
		{
			_camTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;

			_shakeDuration -= Time.deltaTime * _decreaseFactor;
		}

	}

	public void ShakeCamera()
	{
		_originalPos = _camTransform.localPosition;
		_shakeDuration = 0.5f;
	}
}
