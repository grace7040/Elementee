using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[Header("Target Player")]

	public Transform Target;

	//Follow Setting
	bool _enableFollow = true;
	readonly float _targetVerticalScale = 2f;
    readonly float _followSpeed = 3.5f;

    //Shacking
    float _shakeDuration = 0f;  // How long the object should shake for.
    readonly float _shakeAmount = 0.1f;  // Amplitude of the shake. A larger value shakes the camera harder.
    readonly float _decreaseFactor = 1.0f;

	Transform _camTransform;
	Vector3 _originalPos;
	Vector3 _newPosition;

	void Awake()
	{
		Cursor.visible = false;
		_camTransform = GetComponent<Transform>();
	}

	void OnEnable()
	{
		_originalPos = _camTransform.localPosition;
	}

	void Update()
	{
		if (!_enableFollow)
			return;

		FollowTarget();
		UpdateOnShake();
	}

	void FollowTarget()
    {
		_newPosition = Target.position;
		_newPosition.z = -8;
		_newPosition.y += _targetVerticalScale;
		transform.position = Vector3.Slerp(transform.position, _newPosition, _followSpeed * Time.deltaTime);

		_originalPos = transform.position;
	}
	
	public void EnableFollowing(bool value)
    {
		_enableFollow = value;
    }
	public void ShakeCamera()
	{
		_originalPos = _camTransform.localPosition;
		_shakeDuration = 0.5f;
	}

	void UpdateOnShake()
    {
		if (_shakeDuration <= 0) return;

		_camTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;
		_shakeDuration -= Time.deltaTime * _decreaseFactor;
	}
}
