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

    FollowCamera _followCamera;

    [Header("ParticleSystem")]
    public ParticleSystem ParticleJumpUp;
    public ParticleSystem ParticleJumpDown;
    public GameObject HealEffect;

    [Header("Player Properties")]
    public Colors myColor = Colors.Default;
    public SpriteRenderer FaceSprite;
    [HideInInspector] public float AttackCoolTime;
    float _knockBackForce = 10f;
    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    [Header("Health Management")]
    public int CurrentHealth;
    bool _isDie = false;
    int _maxHealth = 100;
    bool _isFountainHealReady = true;
    bool _canGetDamage = true;
    [HideInInspector] public int Damage = 0;

    [Header("Movement Customizing")]
    public float MoveSpeed = 7f;

    [Header("Collision Checking")]
    [SerializeField] LayerMask _whatIsGround;                          
    [SerializeField] Transform _groundCheck;                           
    [SerializeField] Transform _wallCheck;                             
    Collider2D[] _colliders; //colliders : 닿아있는 바닥수만큼 존재, 공중에 떠있으면 0개 바닥에 닿아있으면 1개
    Collider2D[] _collidersWall;

    bool _isGrounded;
    bool _wasGrounded;
    const float _groundedRadius = .2f; // Radius of the overlap circle to determine if grounded


    [Header("Events")]
    [Space]
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    // Move
    bool _facingRight = true;  // For determining which way the player is currently facing.
    float _limitFallSpeed = 25f;
    float _accelerationRate = 5f;
    bool _isRoll = false;

    // Jump
    float _jumpForce = 850f;
    bool _canDoubleJump = true;


    // WallSliding
    bool _isWall = false;
    bool _isWallSliding = false;
    bool _canWallSliding = false;
    bool _limitVelOnWallJump = false; //For limit wall jump distance with low fps
    float _jumpWallDistX = 0; //Distance between player and wall
    float _jumpWallStartX = 0;
    bool _wasWallSlidding = false; //If player is sliding in a wall in the previous frame

    // Dash
    bool _isDashing = false;
    bool _canDash = true;
    float _dashForce = 25f;

    // Rope
    FixedJoint2D _fixJoint;
    bool _isRope = false;
    
    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fixJoint = GetComponent<FixedJoint2D>();
        CurrentHealth = _maxHealth;
        _followCamera = FindObjectOfType<FollowCamera>();
        ColorManager.Instance.InitPlayer(this, SetAnimatorBool, ShakeCamera);

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        ColorManager.Instance.SetColorState(Colors.Purple);
    }


    private void FixedUpdate()
    {
        if (!_isRope)
        {
            _wasGrounded = _isGrounded;
            _isGrounded = false;

            //_isGrounded를 판단하기 위한 콜리전 체크 로직
            _colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i].gameObject != gameObject)
                {
                    _isGrounded = true;

                    //최초로 땅에 착지한 경우
                    if (!_wasGrounded)
                    {
                        OnLandEvent.Invoke();
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
        if (!_isGrounded) //땅에 닿아있지 않을 때 (위에서 오버랩 체킹 했는데 하나도 안 걸린 경우)
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true

            //벽에 닿아있는지 체크
            _collidersWall = Physics2D.OverlapCircleAll(_wallCheck.position, _groundedRadius, _whatIsGround);
            for (int i = 0; i < _collidersWall.Length; i++)
            {
                if (_collidersWall[i].gameObject != null) //벽에 닿아있다면
                {
                    _isDashing = false;
                    _isWall = true;
                    break;
                }
            }
        }

        //벽타기
        if (_limitVelOnWallJump)
        {
            if (_rigidbody.velocity.y < -0.5f)
                _limitVelOnWallJump = false;

            _jumpWallDistX = (_jumpWallStartX - transform.position.x) * transform.localScale.x;

            if (_jumpWallDistX < -1f && _jumpWallDistX >= -2f)
            {
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

        _isRoll = _isGrounded && Math.Abs(_rigidbody.velocity.x) > 9f;
    }



    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
    public void Move(float moveInput, bool jumpInput, bool dashInput)
    {
        if (_isDie) return;

        // Dash
        if (dashInput && _canDash && !_isWallSliding)
        {
            StartCoroutine(DashCooldown());
        }
        if (_isDashing)
        {
            _rigidbody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);
        }
        else
        {
            Walk(moveInput);
        }

        _animator.SetBool("IsRolling", _isRoll);

        // 1단 점프
        if (jumpInput && _isGrounded)
        {
            Jump();
        }
        // 2단 점프
        else if(jumpInput && !_isGrounded && _canDoubleJump && !_isWallSliding)
        {
            DoubleJump();
        }

        //Wall Sliding: 벽이 근처에 있고, wallsliding이 가능하며, 땅에 닿아있지 않을 때(점프시 or 공중)
        else if (_isWall && _canWallSliding && !_isGrounded)
        {
            // 떨어지고 있을 때 or Dash 중일 때
            if (!_wasWallSlidding && _rigidbody.velocity.y < 0 || _isDashing)// || _isDashing
            {
                _isWallSliding = true;
                _wallCheck.localPosition = new Vector3(-_wallCheck.localPosition.x, _wallCheck.localPosition.y, 0);
                Flip();
                _canDoubleJump = true;
                _animator.SetBool("IsWallSliding", true);
            }
            _isDashing = false;

            if (_isWallSliding)
            {
                if (moveInput * transform.localScale.x > 0.1f)
                {
                    EndWallSliding();
                }
                else
                {
                    _wasWallSlidding = true;
                    _rigidbody.velocity = new Vector2(-transform.localScale.x * 2, -5);
                }
            }

            // Do jump while WallSliding
            if (jumpInput && _isWallSliding)
            {
                _animator.SetBool("IsJumping", true); 
                _animator.SetBool("JumpUp", true); 
                _rigidbody.velocity = Vector2.zero; 
                _rigidbody.AddForce(new Vector2(transform.localScale.x * _jumpForce * 1.2f, _jumpForce));
                _jumpWallStartX = transform.position.x;
                _limitVelOnWallJump = true;
                EndWallSliding();
            }

            // Do dash while WallSliding
            else if (dashInput && _canDash)
            {
                EndWallSliding();
                StartCoroutine(DashCooldown());
            }
        }

        else if (_isWallSliding && !_isWall)
        {
            EndWallSliding();
        }

    }

    private void Walk(float move)
    {
        if (_rigidbody.velocity.y < -_limitFallSpeed)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_limitFallSpeed);

        Vector2 targetVelocity = new Vector2(move * MoveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity += (targetVelocity - _rigidbody.velocity) * _accelerationRate * Time.fixedDeltaTime;


        if (move > 0 && !_facingRight && !_isWallSliding)
        {
            Flip();
        }
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
        _wasWallSlidding = false;
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

            // 넉백
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.Normalize(transform.position - enemyPos) * 40f * _knockBackForce);

            // UI Update
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
        if (collision.gameObject.CompareTag("DropKill"))
        {
            Die();
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
        _canGetDamage = false;
        yield return new WaitForSeconds(0.4f);
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        yield return new WaitForSeconds(1.1f);
        GameManager.Instance.GameOver();
    }

    public void Revival(Vector3 revivalPos)
    {
        _animator.SetBool("IsDead", false);
        _isDie = false;
        _canGetDamage = true;
        CurrentHealth = _maxHealth;
        UIManager.Instance.ClosePopupUI();
        transform.position = revivalPos;
    }

    void SetAnimatorBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    void ShakeCamera()
    {
        _followCamera.ShakeCamera();
    }

}