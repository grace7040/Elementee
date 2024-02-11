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

    private void Start()
    {
		GameManager.Instance.SetJoystick.Invoke();
    }
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
		controller.Move(horizontalMove * Time.fixedDeltaTime, jumpDown, dash);
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
		dash = true;

	}
	public void DashUp()
	{
		dash = false;
	}

}
