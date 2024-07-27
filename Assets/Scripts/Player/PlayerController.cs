using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private IColorState color;
    public IColorState Color
    {
        get { return color; }
        set
        {
            color = value;
            damage = value.Damage;
            _canWallSliding = value.WallSliding;
            AttackCoolTime = value.CoolTime;
        }
    }

    public Colors myColor = Colors.Default;
    FollowCamera _followCamera;

    [Header("ParticleSystem")]
    public ParticleSystem ParticleJumpUp;
    public ParticleSystem ParticleJumpDown;
    public GameObject HealEffect;
    


    [Header("Player Properties")]
    public int CurrentHealth;
    public float AttackCoolTime;
    bool _canGetDamage = true;
    bool _isFountainHealReady = true;
    float _knockBackForce = 10f;

    public SpriteRenderer FaceSprite;

    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    [Header("Movement Customizing")]
    public float MoveSpeed = 7f;
    float _dashForce = 25f;
    bool _canControlWhileJump = true;
    bool _canWallSliding = false;

    [Header("Collision Checking")]
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared

    //Health
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int damage = 0;


    [Header("Events")]
    [Space]
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private float jumpForce = 850f;

    public bool m_Grounded;
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
    private bool isDie = false;

    //Flip
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    //Move
    private float limitFallSpeed = 25f; // Limit fall speed
    private Vector3 velocity = Vector3.zero;
    private float m_Acceleration = 5f; //가속 임시 변수

    //-점프
    private bool canDoubleJump = true; //If player can double jump
    //-벽타기
    private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
    private bool canCheck = false; //For check if player is wallsliding
    //로프
    public FixedJoint2D fixJoint;
    private bool isRope = false;




    Collider2D[] colliders;
    Collider2D[] collidersWall;



    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        CurrentHealth = maxHealth;
        _followCamera = FindObjectOfType<FollowCamera>();
        ColorManager.Instance.InitPlayer(this, SetAnimatorBool, ShakeCamera);

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        ColorManager.Instance.SetColorState(Colors.Default);
    }


    private void FixedUpdate()
    {
        //colliders : 닿아있는 바닥수만큼 존재, 공중에 떠있으면 0개 바닥에 닿아있으면 1개
        if (!isRope)
        {
            //땅에 닿아 있는지 판별하는 변수
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++) // 이 for문이 왜 필요하지
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                        if (!m_IsWall && !isDashing)
                            ParticleJumpDown.Play();
                        canDoubleJump = true;
                        if (_rigidbody.velocity.y < 0f)
                            limitVelOnWallJump = false;
                    }

                    break;
                }
            }
        }

        m_IsWall = false;
        if (!m_Grounded) //땅에 닿아있지 않을 때 
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true
            collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null) //벽에 닿아있다면
                {
                    isDashing = false;
                    m_IsWall = true;
                    break;
                }
            }
            prevVelocityX = _rigidbody.velocity.x; // 현재 속도를 저장
        }

        if (limitVelOnWallJump) 
        {
            if (_rigidbody.velocity.y < -0.5f) // 이게 몰까
                limitVelOnWallJump = false;
            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                _rigidbody.velocity = new Vector2(10f * transform.localScale.x, _rigidbody.velocity.y);
            }
            else if (jumpWallDistX < -2f)
            {
                limitVelOnWallJump = false;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            }
            else if (jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
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

        //:: FIX ME :: 리팩을 위한 잠시 주석
        //Vector3 yellowScale = YellowWeaponEffect.transform.localScale;
        //yellowScale.x *= -1;
        //YellowWeaponEffect.transform.localScale = yellowScale;
    }

    public void Move(float move, bool jump, bool dash)
    {
        if (isDie) return;

        if (!canMove) return;

        // Dash
        if (dash && canDash && !isWallSliding)
        {
            StartCoroutine(DashCooldown());
        }
        if (isDashing)
        {
            _rigidbody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);
        }

        // NomalMove & Flip
        else if (m_Grounded || _canControlWhileJump)
        {
            NomalMove(move);
        }


        // Jump
        if (m_Grounded && jump)
        {
            Jump();
        }
        else if(!m_Grounded && jump && canDoubleJump && !isWallSliding)
        {
            DoubleJump();
        }

        //Wall Sliding
        else if (_canWallSliding && m_IsWall && !m_Grounded)
        {
            if (!oldWallSlidding && _rigidbody.velocity.y < 0 || isDashing)
            {
                isWallSliding = true;
                m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
                Flip();
                StartCoroutine(WaitToCheck(0.1f));
                canDoubleJump = true;
                _animator.SetBool("IsWallSliding", true);
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
                    _rigidbody.velocity = new Vector2(-transform.localScale.x * 2, -5);
                }
            }

            // Do jump while WallSliding
            if (jump && isWallSliding)
            {
                _animator.SetBool("IsJumping", true); //
                _animator.SetBool("JumpUp", true); //
                _rigidbody.velocity = Vector2.zero; //
                _rigidbody.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
                jumpWallStartX = transform.position.x;
                limitVelOnWallJump = true;
                EndWallSliding();
                canMove = false;
            }

            // Do dash while WallSliding
            else if (dash && canDash)
            {
                EndWallSliding();
                StartCoroutine(DashCooldown());
            }
        }

        else if (isWallSliding && !m_IsWall && canCheck)
        {
            EndWallSliding();
        }

    }

    private void NomalMove(float move)
    {
        if (_rigidbody.velocity.y < -limitFallSpeed)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -limitFallSpeed);

        Vector2 targetVelocity = new Vector2(move * MoveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity += (targetVelocity - _rigidbody.velocity) * m_Acceleration * Time.fixedDeltaTime;


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

    private void Jump()
    {
        _animator.SetBool("IsJumping", true);
        _animator.SetBool("JumpUp", true);
        m_Grounded = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(new Vector2(0f, jumpForce));
        canDoubleJump = true;
        ParticleJumpDown.Play();
        ParticleJumpUp.Play();
    }
    private void DoubleJump()
    {
        canDoubleJump = false;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.AddForce(new Vector2(0f, jumpForce / 1.2f));
        _animator.SetBool("IsDoubleJumping", true);
    }

   private void EndWallSliding()
    {
        isWallSliding = false;
        _animator.SetBool("IsWallSliding", false);
        oldWallSlidding = false;
        m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
        canDoubleJump = true;
    }

    IEnumerator DashCooldown()
    {
        _animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f); // dash 지속시간
        _rigidbody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);
        isDashing = false;
        yield return new WaitForSeconds(0.5f); // dash cooltime
        canDash = true;
    }

    IEnumerator WaitToCheck(float time)
    {
        canCheck = false;
        yield return new WaitForSeconds(time);
        canCheck = true;
    }

    IEnumerator WaitToEndSliding()
    {
        yield return new WaitForSeconds(0.1f);
        canDoubleJump = true;
        isWallSliding = false;
        _animator.SetBool("IsWallSliding", false);
        oldWallSlidding = false;
        m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
    }

    public void Die()
    {
        if (!isDie)
            StartCoroutine(WaitToDead());
    }

    public void TakeDamage(int damage, Vector3 enemyPos)
    {
        if (_canGetDamage)
        {
            _animator.SetBool("Hit", true);
            CurrentHealth -= damage;

            if (CurrentHealth > 100)
                CurrentHealth = 100;

            //넉백
            Vector2 damageDir = Vector3.Normalize(transform.position - enemyPos) * 40f;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(damageDir * _knockBackForce);

            //UI Update
            GameManager.Instance.UIGame.UpdateHPBar(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                _canGetDamage = false;
                _spriteRenderer.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);
                this.CallOnDelay(1f, () => { _canGetDamage = true; });
            }
        }

    }

    public void Heal(int health)
    {
        CurrentHealth += health;
        AudioManager.Instacne.PlaySFX("Heal");
        if (CurrentHealth > 100)
            CurrentHealth = 100;

        HealEffect.SetActive(true);
        this.CallOnDelay(1f, () => { HealEffect.SetActive(false); });

        //UI Update
        GameManager.Instance.UIGame.UpdateHPBar(CurrentHealth, maxHealth);

    }

    public void HealByFountain(int health)
    {
        if (!_isFountainHealReady) return;

        Heal(health);
        _isFountainHealReady = false;
        this.CallOnDelay(1f, () => { _isFountainHealReady = true; });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<MonsterController>().Damage, collision.gameObject.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<MonsterController>().Damage, collision.gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            if (isRope) return;
            fixJoint.enabled = true;
            fixJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            isRope = true;

            m_Grounded = true;

        }
        
        if (collision.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(15, collision.gameObject.transform.position);
        }
        else if (collision.gameObject.CompareTag("EnemyFarWeapon"))
        {
            TakeDamage(10, collision.gameObject.transform.position);
        }
    }

    public void RopeOut()
    {
        if (!isRope) return;

        fixJoint.connectedBody = null;
        fixJoint.enabled = false;

        this.CallOnDelay(0.1f, () => { isRope = false; });
    }

    IEnumerator WaitToDead()
    {
        _animator.SetBool("IsDead", true);
        isDie = true;
        canMove = false;
        _canGetDamage = false;
        yield return new WaitForSeconds(0.4f);
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        yield return new WaitForSeconds(1.1f);
        GameManager.Instance.GameOver();
    }

    public void Revival()
    {
        _animator.SetBool("IsDead", false);
        isDie = false;
        canMove = true;
        _canGetDamage = true;
        CurrentHealth = maxHealth;
        UIManager.Instance.ClosePopupUI();
    }


    //Effect


    void SetAnimatorBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }




    void ShakeCamera()
    {
        _followCamera.ShakeCamera();
    }

}