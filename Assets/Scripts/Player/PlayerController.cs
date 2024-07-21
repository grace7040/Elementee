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
    public GameObject Camera;

    [Header("ParticleSystem")]
    public ParticleSystem ParticleJumpUp; 
    public ParticleSystem ParticleJumpDown; 
    public GameObject HealEffect;
    public GameObject PurpleAttackEffect;


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
    float _moveSpeed = 7f;
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

    // weapon position
    public GameObject WeaponPosition;

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

    // Black
    private float pullForce = 10f; // 끌어당기는 힘 조절용 변수
    private float throwForce = 15f; // 던지는 힘 조절용 변수
    public bool IsHoldingEnemy = false; // 적을 가지고 있는지 여부
    private Rigidbody2D heldEnemyRigidbody; // 가지고 있는 적의 Rigidbody2D
    private GameObject Enemy;

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

        ColorManager.Instance.InitPlayer(this, SetAnimatorBool);

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        ColorManager.Instance.SetColorState(Colors.Default);
    }

    private void Update()
    {
        // Black관련 코드, 추후 생사 여부 결정
        if (myColor == Colors.Black)
        {
            if (IsHoldingEnemy)
            {
                Enemy.transform.localPosition = new Vector2(0, 0);
            }
        }
        else if (myColor != Colors.Black)
        {
            Destroy(Enemy, 0.1f);
        }
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

        if (limitVelOnWallJump) // 벽과 관련된 듯한데 일단 패스
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

        if (canMove)
        {
            if (dash && canDash && !isWallSliding)
            {
                StartCoroutine(DashCooldown());
            }

            // If crouching, check to see if the character can stand up
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
            if(jump)
            {
                if (m_Grounded)
                {
                    Jump();
                }
                else if(canDoubleJump && !isWallSliding)
                {
                    DoubleJump();
                }
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

                if (jump && isWallSliding)
                {
                    _animator.SetBool("IsJumping", true);
                    _animator.SetBool("JumpUp", true);
                    _rigidbody.velocity = new Vector2(0f, 0f);
                    _rigidbody.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
                    jumpWallStartX = transform.position.x;
                    limitVelOnWallJump = true;
                    canDoubleJump = true;
                    isWallSliding = false;
                    _animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canMove = false;
                }
                else if (dash && canDash)
                {
                    isWallSliding = false;
                    _animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canDoubleJump = true;
                    StartCoroutine(DashCooldown());
                }
            }
            else if (isWallSliding && !m_IsWall && canCheck)
            {
                isWallSliding = false;
                _animator.SetBool("IsWallSliding", false);
                oldWallSlidding = false;
                m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                canDoubleJump = true;
            }

        }
    }

    private void NomalMove(float move)
    {
        if (_rigidbody.velocity.y < -limitFallSpeed)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -limitFallSpeed);

        Vector2 targetVelocity = new Vector2(move * _moveSpeed, _rigidbody.velocity.y);
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

    IEnumerator DashCooldown()
    {
        _animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f); // dash 지속시간
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
            GameManager.Instance.UIGame.UpdateHPBar(CurrentHealth,maxHealth);

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (myColor == Colors.Black)
            {
                if (!collision.gameObject.GetComponent<MonsterController>().isActiveAndEnabled)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    collision.gameObject.GetComponent<OB_VerticlaMovement>().enabled = true;

                    Transform parentTransform = WeaponPosition.transform;
                    Transform childTransform = collision.gameObject.transform;
                    childTransform.SetParent(parentTransform);

                    IsHoldingEnemy = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("EnemyWeapon"))
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
    public void PurpleAttackEffectOn()
    {
        StartCoroutine(Purple_Effect_Set_Active());
    }

    IEnumerator Purple_Effect_Set_Active()
    {
        PurpleAttackEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PurpleAttackEffect.SetActive(false);
    }

    void SetAnimatorBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }


    // Black Attack 유기
    public void BlackPull()
    {
        StartCoroutine(PullCoroutine());
    }

    private IEnumerator PullCoroutine()
    {
        if (!IsHoldingEnemy)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = 7.5f;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestEnemy != null)
            {
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
                Enemy = closestEnemy.gameObject;

                Enemy.SendMessage("PulledByBlack");
                //closestEnemy.AddComponent<BloodEffect>();

                float distance = Vector2.Distance(Enemy.transform.position, transform.position);

                while (!IsHoldingEnemy)
                {
                    Vector2 throwDirection = (transform.position - Enemy.transform.position).normalized;
                    Enemy.transform.Translate(pullForce * Time.deltaTime * throwDirection);

                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(0f);
    }

    public void BlackThrow()
    {
        Rigidbody2D rb = Enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Transform childTransform = Enemy.gameObject.transform;

            childTransform.SetParent(null);

            IsHoldingEnemy = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - transform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB";
            heldEnemyRigidbody = null;

            Enemy.GetComponent<MonsterController>().Die();
        }
    }

}