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
            Damage = value.Damage;
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
    [SerializeField] LayerMask _whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] Transform _groundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] Transform _wallCheck;                             //Posicion que controla si el personaje toca una pared

    //Health
    [HideInInspector] int _maxHealth = 100;
    [HideInInspector] public int Damage = 0;


    [Header("Events")]
    [Space]
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    float _jumpForce = 850f;

    bool _isGrounded;
    const float _groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    bool _isWall = false;

    bool _isWallSliding = false;
    bool _isDashing = false;
    float _prevVelocityX = 0f;

    bool _limitVelOnWallJump = false; //For limit wall jump distance with low fps
    float _jumpWallDistX = 0; //Distance between player and wall
    float _jumpWallStartX = 0;

    bool _canMove = true; 
    bool _canDash = true;
    bool _isDie = false;

    //Flip
    bool _facingRight = true;  // For determining which way the player is currently facing.

    // Move
    float _limitFallSpeed = 25f; 
    float _accelerationRate = 5f; 

    // Jump
    bool _canDoubleJump = true; 

    // WallSliding
    bool _oldWallSlidding = false; //If player is sliding in a wall in the previous frame
    bool _canCheck = false; //For check if player is wallsliding

    // Rope
    FixedJoint2D _fixJoint;
    bool _isRope = false;

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

        CurrentHealth = _maxHealth;
        _followCamera = FindObjectOfType<FollowCamera>();
        ColorManager.Instance.InitPlayer(this, SetAnimatorBool, ShakeCamera);

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        ColorManager.Instance.SetColorState(Colors.Default);

        _fixJoint = GetComponent<FixedJoint2D>();
    }


    private void FixedUpdate()
    {
        //colliders : 닿아있는 바닥수만큼 존재, 공중에 떠있으면 0개 바닥에 닿아있으면 1개
        if (!_isRope)
        {
            //땅에 닿아 있는지 판별하는 변수
            bool wasGrounded = _isGrounded;
            _isGrounded = false;

            colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
            for (int i = 0; i < colliders.Length; i++) // 이 for문이 왜 필요하지
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _isGrounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                        if (!_isWall && !_isDashing)
                            ParticleJumpDown.Play();
                        _canDoubleJump = true;
                        if (_rigidbody.velocity.y < 0f)
                            _limitVelOnWallJump = false;
                    }

                    break;
                }
            }
        }

        _isWall = false;
        if (!_isGrounded) //땅에 닿아있지 않을 때 
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true
            collidersWall = Physics2D.OverlapCircleAll(_wallCheck.position, _groundedRadius, _whatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null) //벽에 닿아있다면
                {
                    _isDashing = false;
                    _isWall = true;
                    break;
                }
            }
            _prevVelocityX = _rigidbody.velocity.x; // 현재 속도를 저장
        }

        if (_limitVelOnWallJump) 
        {
            if (_rigidbody.velocity.y < -0.5f) // 이게 몰까
                _limitVelOnWallJump = false;
            _jumpWallDistX = (_jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (_jumpWallDistX < -0.5f && _jumpWallDistX > -1f)
            {
                _canMove = true;
            }
            else if (_jumpWallDistX < -1f && _jumpWallDistX >= -2f)
            {
                _canMove = true;
                _rigidbody.velocity = new Vector2(10f * transform.localScale.x, _rigidbody.velocity.y);
            }
            else if (_jumpWallDistX < -2f)
            {
                _limitVelOnWallJump = false;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            }
            else if (_jumpWallDistX > 0)
            {
                _limitVelOnWallJump = false;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            }
        }
    }



    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

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
        if (_isDie) return;

        if (!_canMove) return;

        // Dash
        if (dash && _canDash && !_isWallSliding)
        {
            StartCoroutine(DashCooldown());
        }
        if (_isDashing)
        {
            _rigidbody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);
        }

        // NomalMove & Flip
        else if (_isGrounded || _canControlWhileJump)
        {
            NomalMove(move);
        }
        
        if(_isGrounded && Math.Abs(_rigidbody.velocity.x)> 8.4f && !_isDashing)
        {
            print("구르기");
            //_animator.SetBool("IsRolling", true);
        }


        // Jump
        if (_isGrounded && jump)
        {
            Jump();
        }
        else if(!_isGrounded && jump && _canDoubleJump && !_isWallSliding)
        {
            DoubleJump();
        }

        //Wall Sliding
        else if (_canWallSliding && _isWall && !_isGrounded)
        {
            if (!_oldWallSlidding && _rigidbody.velocity.y < 0 || _isDashing)
            {
                _isWallSliding = true;
                _wallCheck.localPosition = new Vector3(-_wallCheck.localPosition.x, _wallCheck.localPosition.y, 0);
                Flip();
                StartCoroutine(WaitToCheck(0.1f));
                _canDoubleJump = true;
                _animator.SetBool("IsWallSliding", true);
            }
            _isDashing = false;

            if (_isWallSliding)
            {
                if (move * transform.localScale.x > 0.1f)
                {
                    StartCoroutine(WaitToEndSliding());
                }
                else
                {
                    _oldWallSlidding = true;
                    _rigidbody.velocity = new Vector2(-transform.localScale.x * 2, -5);
                }
            }

            // Do jump while WallSliding
            if (jump && _isWallSliding)
            {
                _animator.SetBool("IsJumping", true); //
                _animator.SetBool("JumpUp", true); //
                _rigidbody.velocity = Vector2.zero; //
                _rigidbody.AddForce(new Vector2(transform.localScale.x * _jumpForce * 1.2f, _jumpForce));
                _jumpWallStartX = transform.position.x;
                _limitVelOnWallJump = true;
                EndWallSliding();
                _canMove = false;
            }

            // Do dash while WallSliding
            else if (dash && _canDash)
            {
                EndWallSliding();
                StartCoroutine(DashCooldown());
            }
        }

        else if (_isWallSliding && !_isWall && _canCheck)
        {
            EndWallSliding();
        }

    }

    private void NomalMove(float move)
    {
        if (_rigidbody.velocity.y < -_limitFallSpeed)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_limitFallSpeed);

        Vector2 targetVelocity = new Vector2(move * MoveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity += (targetVelocity - _rigidbody.velocity) * _accelerationRate * Time.fixedDeltaTime;


        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !_facingRight && !_isWallSliding)
        {
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && _facingRight && !_isWallSliding)
        {
            Flip();
        }
    }

    private void Jump()
    {
        _animator.SetBool("IsJumping", true);
        _animator.SetBool("JumpUp", true);
        _isGrounded = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(new Vector2(0f, _jumpForce));
        _canDoubleJump = true;
        ParticleJumpDown.Play();
        ParticleJumpUp.Play();
    }
    private void DoubleJump()
    {
        _canDoubleJump = false;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.AddForce(new Vector2(0f, _jumpForce / 1.2f));
        _animator.SetBool("IsDoubleJumping", true);
    }

   private void EndWallSliding()
    {
        _isWallSliding = false;
        _animator.SetBool("IsWallSliding", false);
        _oldWallSlidding = false;
        _wallCheck.localPosition = new Vector3(Mathf.Abs(_wallCheck.localPosition.x), _wallCheck.localPosition.y, 0);
        _canDoubleJump = true;
    }

    IEnumerator DashCooldown()
    {
        _animator.SetBool("IsDashing", true);
        _isDashing = true;
        _canDash = false;
        yield return new WaitForSeconds(0.1f); // dash 지속시간
        _rigidbody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);
        _isDashing = false;
        yield return new WaitForSeconds(0.5f); // dash cooltime
        _canDash = true;
    }

    IEnumerator WaitToCheck(float time)
    {
        _canCheck = false;
        yield return new WaitForSeconds(time);
        _canCheck = true;
    }

    IEnumerator WaitToEndSliding()
    {
        yield return new WaitForSeconds(0.1f);
        _canDoubleJump = true;
        _isWallSliding = false;
        _animator.SetBool("IsWallSliding", false);
        _oldWallSlidding = false;
        _wallCheck.localPosition = new Vector3(Mathf.Abs(_wallCheck.localPosition.x), _wallCheck.localPosition.y, 0);
    }

    public void Die()
    {
        if (!_isDie)
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
            GameManager.Instance.UIGame.UpdateHPBar(CurrentHealth, _maxHealth);

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
        GameManager.Instance.UIGame.UpdateHPBar(CurrentHealth, _maxHealth);

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
            if (_isRope) return;
            _fixJoint.enabled = true;
            _fixJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            _isRope = true;

            _isGrounded = true;

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
        if (!_isRope) return;

        _fixJoint.connectedBody = null;
        _fixJoint.enabled = false;

        this.CallOnDelay(0.1f, () => { _isRope = false; });
    }

    IEnumerator WaitToDead()
    {
        _animator.SetBool("IsDead", true);
        _isDie = true;
        _canMove = false;
        _canGetDamage = false;
        yield return new WaitForSeconds(0.4f);
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        yield return new WaitForSeconds(1.1f);
        GameManager.Instance.GameOver();
    }

    public void Revival()
    {
        _animator.SetBool("IsDead", false);
        _isDie = false;
        _canMove = true;
        _canGetDamage = true;
        CurrentHealth = _maxHealth;
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