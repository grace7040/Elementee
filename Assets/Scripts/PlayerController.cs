using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private ColorState color;
    public ColorState Color
    {
        get { return color; }
        set { 
            color = value;
            m_JumpForce = value.JumpForce; 
        }
    }

    [Header("Movement Customizing")]
     private float m_JumpForce;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private float m_DashForce = 25f;
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;


    [Header("Collision Checking")]
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared


    [Header("ParticleSystem")]
    public ParticleSystem particleJumpUp; //Trail particles
    public ParticleSystem particleJumpDown; //Explosion particles

    //
    [Header("Events")]
    [Space]

    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    ////////


    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;

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
    //-점프
    private bool canDoubleJump = true; //If player can double jump
    //-벽타기
    private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
    private bool canSlide = false; //For check if player is wallsliding


    private void Start()
    {
        Color = new DefaultColor();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (OnFallEvent == null) // 이건 왜하지?
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {

        //땅에 닿아 있는지 판별하는 변수
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        //print(m_Grounded);

        //colliders : 닿아있는 바닥수만큼 존재, 공중에 떠있으면 0개 바닥에 닿아있으면 1개
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) // 이 for문이 왜 필요하지
        {
            //gameobject = plsyer, colliders[i].gameObject = player와 접촉하고 있는 obj
            if (colliders[i].gameObject != gameObject)

            {
                m_Grounded = true;
                if (!wasGrounded) //여기 아직 안 봄
                {
                    OnLandEvent.Invoke();
                    if (!m_IsWall && !isDashing)
                        particleJumpDown.Play();
                    canDoubleJump = true;
                    if (m_Rigidbody2D.velocity.y < 0f)
                        limitVelOnWallJump = false;
                }
            }

        }


        m_IsWall = false;
        if (!m_Grounded) //땅에 닿아있지 않을 때 
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true
            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null) //벽에 닿아있다면
                {
                    isDashing = false;
                    m_IsWall = true;
                }
            }
            prevVelocityX = m_Rigidbody2D.velocity.x; // 현재 속도를 저장
        }


        if (limitVelOnWallJump) // 벽과 관련된 듯한데 일단 패스
        {
            if (m_Rigidbody2D.velocity.y < -0.5f) // 이게 몰까
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
    /// <param name="dash"> dash 를 눌렀을 때 true </param>
    public void Move(float move, bool jump, bool dash)
    {
        if (canMove)
        {
            Dash(move, jump, dash);
            Jump(jump, dash);

            //Wallsliding
            //플레이어 앞에 벽이 있고 땅에 닿아있지 않을
            /**
            else if (m_IsWall && !m_Grounded)
            {
                //이전프레임에서 벽을 안 탔고 fallingf중일때 or dash중일 때
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




    }

    private void Dash(float move, bool jump, bool dash)
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

        //isDashing: false & ( 땅에 닿아있음 or 공중에 떠 있는 중 컨트롤이 가능할 때 )
        else if (m_Grounded || m_AirControl)
        {
            // 1) 하강 속도가 제한보다 빠르지 않도록 조정
            if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);

            // 2) 떠 있을 때 현재로부터 target velocity 까지 스무스하게 내려오도록 설정
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y); //move에 10f는 왜 곱하지?
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);


            //3) 내가 누르는것과 반대로 플레이어가 움직이고 있는 경우 바꿔준다.
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight && !isWallSliding)
            {
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight && !isWallSliding)
            {
                Flip();
            }
        }

    }

    private void Jump(bool jump, bool dash)
    {
        //2. JUMP
        // 땅에 닿아있을 때 jump를 누르면
        if (m_Grounded && jump) // do jump
        {
            // Add a vertical force to the player.
            animator.SetBool("IsJumping", true);
            animator.SetBool("JumpUp", true);
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            canDoubleJump = true;
            particleJumpDown.Play();
            particleJumpUp.Play();
        }
        //땅에서 떠 있는데 점프를 할 수 있는 상태 + 벽을 타고 있지 않으면 => 더블 점프
        else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
        {
            canDoubleJump = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
            animator.SetBool("IsDoubleJumping", true);
        }

        //else if (!m_Grounded && dash && canDash)
        //{
        //    //m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
        //    //canDoubleJump = true;
        //    StartCoroutine(DashCooldown());
        //}
    }


    IEnumerator DashCooldown()
    {
        animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f); // dash 지속시간
        isDashing = false;
        yield return new WaitForSeconds(0.5f); // dash cooltime
        canDash = true;
    }

    //Wall slide cooroutine
    /**
    //canSlide인지 확인, canSlide : 플레이어가 벽을 타고 있는지
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
