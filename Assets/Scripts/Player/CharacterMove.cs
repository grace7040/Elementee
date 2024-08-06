using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
	public VariableJoystick joystick;

	private float runSpeed = 60f;

	private float horizontalMove = 0f;

	private bool dashDown = false;
	private bool jumpDown = false;

	PlayerAttack _playerAttack;
	PlayerController _playerController;
	Animator _animator;

	private void Start()
    {
		_playerController = GetComponent<PlayerController>();
		_playerAttack = GetComponent<PlayerAttack>();
		_animator = GetComponent<Animator>();
    }
    void Update()
	{
		horizontalMove = joystick.Horizontal * runSpeed;

		_animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Z))
			JumpDown();

		if (Input.GetKeyDown(KeyCode.C))
			DashDown();

		if (Input.GetKeyDown(KeyCode.X))
			_playerAttack.AttackDown();

	}

	void FixedUpdate()
	{
		// Move our character
		_playerController.Move(horizontalMove * Time.fixedDeltaTime, jumpDown, dashDown);
		JumpUp();
		DashUp();
	}


	public void JumpDown()
	{
		jumpDown = true;
		_playerController.RopeOut();
	}
	public void JumpUp()
	{
		jumpDown = false;
	}
	public void DashDown()
	{
		dashDown = true;
	}
	public void DashUp()
	{
		dashDown = false;
	}

}