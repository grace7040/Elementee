using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
	[SerializeField] private PlayerController controller;
	[SerializeField] private Animator animator;
	public VariableJoystick joystick; // 확인 필요

	private float runSpeed = 60f;

	private float horizontalMove = 0f;

	private bool dashDown = false;
	private bool jumpDown = false;

    void Update()
	{
		horizontalMove = joystick.Horizontal * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Z))
        {
			JumpDown();
        }

        if (Input.GetKeyDown(KeyCode.C))
		{
			DashDown();
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
		controller.Move(horizontalMove * Time.fixedDeltaTime, jumpDown, dashDown);
		JumpUp();
		DashUp();
	}


	public void JumpDown()
	{
		jumpDown = true;
		controller.RopeOut();
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
