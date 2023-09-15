using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CharacterController2 : MonoBehaviour
{
    //���� ���� ���� ��
    [Header("Events")]
    [Space]
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared
    [SerializeField] private float m_DashForce = 25f;


    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    public ParticleSystem particleJumpUp; //Trail particles
    public ParticleSystem particleJumpDown; //Explosion particles

    private float m_MovementSmoothing = .05f;  // How much to smooth out the movement


    private bool m_Grounded;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    private bool m_IsWall = false; //If there is a wall in front of the player
    private bool isDashing = false; //If player is dashing

    private float prevVelocityX = 0f;


    private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps
    private float jumpWallDistX = 0; //Distance between player and wall
    private float jumpWallStartX = 0;
    private bool isWallSliding = false; //If player is sliding in a wall

    private bool canMove = true; //If player can move
    private bool canDash = true;


    //Flip
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    //Move
    private float limitFallSpeed = 25f; // Limit fall speed
    private Vector3 velocity = Vector3.zero;
    //-����
    public bool canDoubleJump = true; //If player can double jump
    //-��Ÿ��
    private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
    private bool canSlide = false; //For check if player is wallsliding


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (OnFallEvent == null) // �̰� ������?
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        print(m_AirControl);
        //���� ��� �ִ��� �Ǻ��ϴ� ����
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        //colliders : ����ִ� �ٴڼ���ŭ ����, ���߿� �������� 0�� �ٴڿ� ��������� 1��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) // �� for���� �� �ʿ�����
        {
            //gameobject = plsyer, colliders[i].gameObject = player�� �����ϰ� �ִ� obj
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
            if (!wasGrounded) //���� ���� �� ��
            {
                OnLandEvent.Invoke();
                if (!m_IsWall && !isDashing)
                    particleJumpDown.Play();
                canDoubleJump = true;
                if (m_Rigidbody2D.velocity.y < 0f)
                    limitVelOnWallJump = false;
            }
        }


        m_IsWall = false;
        if (!m_Grounded) //���� ������� ���� �� 
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true
            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null) //���� ����ִٸ�
                {
                    isDashing = false;
                    m_IsWall = true;
                }
            }
            prevVelocityX = m_Rigidbody2D.velocity.x; // ���� �ӵ��� ����
        }


        if (limitVelOnWallJump) // ���� ���õ� ���ѵ� �ϴ� �н�
        {
            if (m_Rigidbody2D.velocity.y < -0.5f) // �̰� ����
                limitVelOnWallJump = false;
            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX < -2f)
            {
                limitVelOnWallJump = false;
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            }
        }




    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    /// <param name="jump"></param>
    /// <param name="dash"> dash �� ������ �� true </param>
    public void Move(float move, bool jump, bool dash)
    {
        if (canMove)
        {
            //1. dash ON
            if (dash && canDash && !isWallSliding)
            {
                StartCoroutine(DashCooldown());
            }
            // do dashing
            if (isDashing)
            {
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }

            //isDashing: false & ( ���� ������� or ���߿� �� �ִ� �� ��Ʈ���� ������ �� )
            else if (m_Grounded || m_AirControl)
            {
                // 1) �ϰ� �ӵ��� ���Ѻ��� ������ �ʵ��� ����
                if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);

                // 2) �� ���� �� ����κ��� target velocity ���� �������ϰ� ���������� ����
                Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y); //move�� 10f�� �� ������?
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);


                //3) ���� �����°Ͱ� �ݴ�� �÷��̾ �����̰� �ִ� ��� �ٲ��ش�.
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight && !isWallSliding)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight && !isWallSliding)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            //2. JUMP
            // ���� ������� �� jump�� ������
            if (m_Grounded && jump) // do jump
            {
                // Add a vertical force to the player.
                animator.SetBool("IsJumping", true);
                //animator.SetBool("JumpUp", true);
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                canDoubleJump = true;
                particleJumpDown.Play();
                particleJumpUp.Play();
            }
            //������ �� �ִµ� ������ �� �� �ִ� ���� + ���� Ÿ�� ���� ������ => ���� ����
            else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
            {
                canDoubleJump = false;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
                animator.SetBool("IsDoubleJumping", true);
            }

            //Wallsliding
            //�÷��̾� �տ� ���� �ְ� ���� ������� ����
            /**
            else if (m_IsWall && !m_Grounded)
            {
                //���������ӿ��� ���� �� ���� fallingf���϶� or dash���� ��
                if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
                {
                    isWallSliding = true;
                    m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
                    Flip();
                    StartCoroutine(WaitToCheck(0.1f));
                    canDoubleJump = true;
                    animator.SetBool("IsWallSliding", true);
                }
                isDashing = false;

                if (isWallSliding)
                {
                    if (move * transform.localScale.x > 0.1f)
                    {
                        StartCoroutine(WaitToEndSliding());
                    }
                    else
                    {
                        oldWallSlidding = true;
                        m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
                    }
                }

                if (jump && isWallSliding)
                {
                    animator.SetBool("IsJumping", true);
                    animator.SetBool("JumpUp", true);
                    m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                    m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce * 1.2f, m_JumpForce));
                    jumpWallStartX = transform.position.x;
                    limitVelOnWallJump = true;
                    canDoubleJump = true;
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canMove = false;
                }
                else if (dash && canDash)
                {
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canDoubleJump = true;
                    StartCoroutine(DashCooldown());
                }
            }
            else if (isWallSliding && !m_IsWall && canSlide)
            {
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                oldWallSlidding = false;
                m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                canDoubleJump = true;
            }
            **/


        }



        IEnumerator DashCooldown()
        {
            animator.SetBool("IsDashing", true);
            isDashing = true;
            canDash = false;
            yield return new WaitForSeconds(0.1f); // dash ���ӽð�
            isDashing = false;
            yield return new WaitForSeconds(0.5f); // dash cooltime
            canDash = true;
        }

        //Wall slide cooroutine
        /**
        //canSlide���� Ȯ��, canSlide : �÷��̾ ���� Ÿ�� �ִ���
        IEnumerator WaitToCheck(float time)
        {
            canSlide = false;
            yield return new WaitForSeconds(time);
            canSlide = true;
        }

        IEnumerator WaitToEndSliding()
        {
            yield return new WaitForSeconds(0.1f);
            canDoubleJump = true;
            isWallSliding = false;
            animator.SetBool("IsWallSliding", false);
            oldWallSlidding = false;
            m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
        }
        **/

    }

}
