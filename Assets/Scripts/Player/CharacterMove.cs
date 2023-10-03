using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
	public PlayerController controller;
	public Animator animator;
	public VariableJoystick joystick;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool dash = false;

	private bool jumpDown = false;

	void Update()
	{
		horizontalMove = joystick.Horizontal * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		//if (Input.GetKeyDown(KeyCode.Z))
		//{
		//	jump = true;
		//}

		if (Input.GetKeyDown(KeyCode.C))
		{
			dash = true;
		}
	

	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jumpDown, dash);
		jumpDown = false;
		dash = false;
	}


	public void JumpDown()
	{
		jumpDown = true;

	}
	public void JumpUp()
	{
		jumpDown = false;
	}

}
