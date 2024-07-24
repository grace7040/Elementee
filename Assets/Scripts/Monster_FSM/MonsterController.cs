using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading;

public class MonsterController : MonoBehaviour
{
    #region variables

    public Monster MonsterData;
    public Transform MonsterBody;
    private SpriteRenderer _monsterSprite;
    private GameObject _playerObj;

    [HideInInspector]
    public Colors MyColor;

    [HideInInspector]
    public Transform Player;

    [HideInInspector]
    public Rigidbody2D Rb;

    public Animator Animator;

    private GameObject _spark;

    private int _maxHealth;
    private int _currentHealth;

    [HideInInspector]
    public float MoveSpeed = 3f;

    [HideInInspector]
    public float DetectionRange = 10f;

    [HideInInspector]
    public float AttackRange;

    [HideInInspector]
    public bool IsDie = false;

    [HideInInspector]
    public int Damage;

    // Cooltime
    private bool _canTakeDamage_RangeAttack = true;

    private Vector3 _leftEndpoint;
    private Vector3 _rightEndpoint;

    [HideInInspector]
    public Vector3 CurrentEndpoint;

    private float _EndpointDirection;
    private float _distanceX;

    [HideInInspector]
    public float DistanceY;

    [HideInInspector]
    public Quaternion FlipQuaternion = Quaternion.Euler(new Vector3(0, 180, 0));

    protected Vector2 dir;

    private float _moveRange = 50.0f;
    private float _stopTime = 0;
    private float _timeSinceLastStop = 0;
    private bool _isStopped = false;
    private bool _canMove = true;
    private float _timer = 0.0f;
    private float _interval = 5.0f;

    // Knockback
    private bool _isKnockedBack = false;

    [HideInInspector]
    public bool IsFlip = false;

    [HideInInspector]
    public bool CanFlip = true;

    // Die
    private delegate void Del();
    private Del OnDie = null;

    // HP Bar
    private Image _hpBar;
    private Image _hpBarBG;
    private float _hpBarMAX;

    private LayerMask _ignoreLayers;
    public Vector2 MoveDirection;
    private Quaternion _checkRotation;
    private Vector2 _raycastOrigin;
    private RaycastHit2D _hit;

    protected StateMachine stateMachine;

    #endregion

    protected virtual void Awake()
    {
        Damage = MonsterData.Damage;
        _maxHealth = MonsterData.Health;
        MyColor = MonsterData.MyColor;
        AttackRange = MonsterData.AttackRange;
        _currentHealth = _maxHealth;
    }

    protected virtual void Start()
    {
        if (Player == null)
        {
            _playerObj = GameManager.Instance.Player;
            Player = _playerObj.transform;
        }

        _monsterSprite = GetComponentInChildren<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();

        _ignoreLayers = LayerMask.GetMask("Monster", "Player", "TransparentFX", "Coins");

        SetEndpoints();
        CurrentEndpoint = _leftEndpoint;

        _hpBar = transform.Find("HPBar").GetChild(1).gameObject.GetComponent<Image>();
        _hpBarMAX = _hpBar.gameObject.GetComponent<RectTransform>().rect.width;
        _hpBarBG = transform.Find("HPBar").GetChild(0).gameObject.GetComponent<Image>();

        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (IsDie) return;

        _distanceX = Mathf.Abs(transform.position.x - Player.position.x);
        DistanceY = Mathf.Abs(transform.position.y - Player.position.y);

        if (stateMachine.CurrentState is IdleState)
        {
            _EndpointDirection = CurrentEndpoint.x - transform.position.x;
            IsFlip = _EndpointDirection < 0f;
            MonsterBody.rotation = IsFlip ? Quaternion.identity : FlipQuaternion;
        }

        stateMachine.Update();
    }

    public void Move()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            SetEndpoints();
            _timer = 0.0f;
        }

        if (CurrentEndpoint != null && !_isKnockedBack)
        {
            MoveTowardsEndpoint();
        }
    }

    private void SetEndpoints()
    {
        _leftEndpoint = GetEndpoint(Vector2.left, _ignoreLayers);
        _rightEndpoint = GetEndpoint(Vector2.right, _ignoreLayers);
    }

    private Vector3 GetEndpoint(Vector2 direction, LayerMask ignoreLayers)
    {
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, direction, _moveRange, ~ignoreLayers);

        if (_hit.collider != null)
        {
            return _hit.point - (direction * 1.0f);
        }
        else
        {
            return transform.position + (Vector3)(direction * _moveRange / 2);
        }
    }

    private void MoveTowardsEndpoint()
    {
        if (_isStopped)
        {
            HandleStopAndSetNextEndpoint();
        }
        else
        {
            HandleMovement();
        }
    }

    private void HandleStopAndSetNextEndpoint()
    {
        _stopTime -= Time.deltaTime;
        if (_stopTime <= 0)
        {
            _isStopped = false;
            SetNextEndpoint();
        }
    }

    private void HandleMovement()
    {
        _timeSinceLastStop += Time.deltaTime;

        if (_timeSinceLastStop >= 2.0f)
        {
            _canMove = true;
        }

        MoveDirection = (CurrentEndpoint - transform.position).normalized;
        MoveDirection.y = 0;
        Rb.velocity = MoveDirection * MoveSpeed;

        Animator.SetBool("IsWalking", MyColor != Colors.Default);

        if (CheckCliff() || Mathf.Abs(_EndpointDirection) < 0.2f)
        {
            SetNextEndpoint();
        }

        if (_canMove && Random.value < 0.3f * Time.deltaTime)
        {
            _stopTime = Random.Range(0.4f, 1.0f);
            _isStopped = true;
            _canMove = false;
            _timeSinceLastStop = 0;

            Animator.SetBool("IsWalking", MyColor != Colors.Default && false);
        }
    }

    public bool CheckCliff()
    {
        _checkRotation = IsFlip ? FlipQuaternion : Quaternion.identity;
        _raycastOrigin = transform.position + (_checkRotation * Vector3.right * 0.7f);

        //UnityEngine.Debug.DrawRay(raycastOrigin, Vector2.down, Color.red);

        _hit = Physics2D.Raycast(_raycastOrigin, Vector2.down, 1.0f, 1 << 0);

        return _hit.collider == null;
    }

    private void SetNextEndpoint()
    {
        CurrentEndpoint = IsFlip ? _rightEndpoint : _leftEndpoint;
    }

    public bool CheckGround()
    {
        //UnityEngine.Debug.DrawRay(transform.position, Vector2.down * 0.7f, Color.red);

        _hit = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, 1 << 0);

        return _hit.collider != null;
    }

    public void TakeDamage(int damage, Vector3 playerPos)
    {
        _currentHealth -= damage;

        UpdateHPBar();
        PlayDamageEffects();
        DisplayDamageText(damage);

        if (!_isKnockedBack)
        {
            ApplyKnockback(playerPos);
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageEffects()
    {
        _monsterSprite.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);
        this.CallOnDelay(1f, () => { _monsterSprite.DOFade(1f, 0f); });
    }

    private void DisplayDamageText(int damage)
    {
        var damageText = ObjectPoolManager.Instance.GetGo("DamageText");
        damageText.GetComponent<TextMeshPro>().text = damage.ToString();
        damageText.transform.position = transform.position;
        damageText.transform.SetParent(this.transform);
    }

    private void ApplyKnockback(Vector3 playerPos)
    {
        Rb.velocity = Vector2.zero;
        Vector2 damageDir = new Vector2(transform.position.x - playerPos.x, 0).normalized * 2f;
        damageDir += new Vector2(0, 1).normalized * 2f;
        ApplyKnockbackForce(damageDir, 12f, 0.3f);
    }

    public void Die()
    {
        if (IsDie) return;

        IsDie = true;
        switch (MyColor)
        {
            case Colors.Default:
                break;
            case Colors.Red:
                ColorManager.Instance.HasRed = true;
                break;
            case Colors.Blue:
                ColorManager.Instance.HasBlue = true;
                break;
            case Colors.Yellow:
                ColorManager.Instance.HasYellow = true;
                break;
        }

        Animator.enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        Rb.velocity = Vector2.zero;
        dir.x = 0;

        OnDie?.Invoke();

        _monsterSprite.DOFade(0, 2.5f);
        _hpBarBG.DOFade(0, 2f);
        Destroy(gameObject, 2.5f);
    }

    private IEnumerator Electrocuted()
    {
        enabled = false;
        Animator.speed = 0f;

        _spark = ObjectPoolManager.Instance.GetGo("Spark");
        _spark.transform.SetParent(transform);
        _spark.transform.localPosition = Vector3.zero;

        StartCoroutine(ShakeMonster());
        yield return new WaitForSeconds(2.0f);

        enabled = true;
        Animator.speed = 1f;
    }

    private IEnumerator ShakeMonster()
    {
        float shakeDuration = 1.5f;
        float shakeIntensity = 0.05f;

        while (shakeDuration > 0)
        {
            transform.position += new Vector3(Random.Range(-shakeIntensity, shakeIntensity), 0f, 0f);
            shakeDuration -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsDie) return;

        if (other.CompareTag("Weapon"))
        {
            TakeDamage(other.GetComponentInParent<PlayerController>().damage, other.transform.position);
        }
        else if (other.gameObject.CompareTag("WeaponThrow"))
        {
            TakeDamage(35, other.transform.position);
        }
        else if (other.gameObject.CompareTag("WeaponB"))
        {
            TakeDamage(100, other.transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_canTakeDamage_RangeAttack || IsDie) return;

        _canTakeDamage_RangeAttack = false;
        if (collision.CompareTag("WeaponYellow"))
        {
            TakeDamage(15, collision.transform.position);
            StartCoroutine(Electrocuted());
        }
        else if (collision.CompareTag("WeaponOrange"))
        {
            TakeDamage(25, collision.transform.position);
        }

        this.CallOnDelay(0.5f, () => { _canTakeDamage_RangeAttack = true; });
    }

    private void ApplyKnockbackForce(Vector2 direction, float force, float duration)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        float timer = 0f;
        _isKnockedBack = true;

        while (timer < duration)
        {
            Rb.AddForce(force * Time.fixedDeltaTime * direction, ForceMode2D.Impulse);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.8f);
        _isKnockedBack = false;
    }

    private void OnDieByGreenPlayer()
    {
        if (GameManager.Instance.PlayerColor == Colors.Green)
        {
            Instantiate(Resources.Load("Monster/Leaf"), transform.position, Quaternion.identity);
        }
        else
        {
            OnDie -= OnDieByGreenPlayer;
        }
    }

    private void UpdateHPBar()
    {
        _hpBar.fillAmount = (float)_currentHealth / _maxHealth;
    }

    public void SetOnDieByGreenPlayer()
    {
        OnDie = OnDieByGreenPlayer;
    }

    public void PulledByBlack()
    {
        Animator.enabled = false;
        IsDie = true;

        if (MyColor == Colors.Yellow) GetComponent<YellowMonster>().voltObject.SetActive(false);
        else if (MyColor == Colors.Red) GetComponent<RedMonster>().fireObject.SetActive(false);

        enabled = false;
        Rb.mass = 0.0f;
        Rb.gravityScale = 0.0f;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
    }

    public virtual void Attack() { }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}