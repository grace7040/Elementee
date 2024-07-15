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

    [HideInInspector]
    public Colors MyColor;

    protected SpriteRenderer monsterSprite;

    [HideInInspector]
    public Transform Player;

    [HideInInspector]
    public Rigidbody2D Rb;

    protected int maxHealth;
    protected int currentHealth;

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
    protected bool canTakeDamage_RangeAttack = true;

    protected Vector3 leftWaypoint;
    protected Vector3 rightWaypoint;

    [HideInInspector]
    public Vector3 CurrentWaypoint;

    protected float moveRange = 50.0f;
    protected float stopTime = 0;
    protected float timeSinceLastStop = 0;
    protected bool isStopping = false;
    protected bool canMove = true;
    protected float timer = 0.0f;
    protected float interval = 5.0f;

    // 넉백
    protected bool isKnockedBack = false;

    // Die
    public delegate void Del();
    public Del OnDie = null;

    // HP Bar
    protected Image hpBar;
    protected Image hpBarBG;
    protected float hpBarMAX;

    protected float waypointDirection;
    protected float distanceX;

    [HideInInspector]
    public float DistanceY;

    public Animator Animator;

    [HideInInspector]
    public Quaternion FlipQuaternion = Quaternion.Euler(new Vector3(0, 180, 0));

    public Transform MonsterBody;

    [HideInInspector]
    public bool IsFlip = false;

    [HideInInspector]
    public bool CanFlip = true;

    protected bool isGrounded = true;

    protected Vector2 dir;
    protected GameObject playerObj;

    GameObject spark;

    protected StateMachine stateMachine;

    #endregion

    protected virtual void Awake()
    {
        Damage = MonsterData.Damage;
        maxHealth = MonsterData.Health;
        MyColor = MonsterData.MyColor;
        AttackRange = MonsterData.AttackRange;
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        if (Player == null)
        {
            playerObj = GameManager.Instance.player;
            Player = playerObj.transform;
        }

        monsterSprite = GetComponentInChildren<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();

        // waypoint 초기화
        SetWaypoints();    
        CurrentWaypoint = leftWaypoint;

        // 체력바
        hpBar = transform.Find("HPBar").GetChild(1).gameObject.GetComponent<Image>();
        hpBarMAX = hpBar.gameObject.GetComponent<RectTransform>().rect.width;
        hpBarBG = transform.Find("HPBar").GetChild(0).gameObject.GetComponent<Image>();

        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (IsDie) return;

        if (playerObj != null) isGrounded = playerObj.GetComponent<PlayerController>().m_Grounded;

        distanceX = Mathf.Abs(transform.position.x - Player.position.x);
        DistanceY = Mathf.Abs(transform.position.y - Player.position.y);

        if (stateMachine.CurrentState is IdleState)
        {
            waypointDirection = CurrentWaypoint.x - transform.position.x;
            IsFlip = waypointDirection < 0f;
            MonsterBody.rotation = IsFlip ? Quaternion.identity : FlipQuaternion;
        }

        stateMachine.Update();
    }

    public void Move()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            SetWaypoints();
            timer = 0.0f;
        }

        if (CurrentWaypoint != null && !isKnockedBack)
        {
            MoveTowardsWaypoint();
        }
    }

    protected void SetWaypoints()
    {
        LayerMask ignoreLayers = LayerMask.GetMask("Monster", "Player", "TransparentFX", "Coins");

        leftWaypoint = GetWaypoint(Vector2.left, ignoreLayers);
        rightWaypoint = GetWaypoint(Vector2.right, ignoreLayers);
    }

    protected Vector3 GetWaypoint(Vector2 direction, LayerMask ignoreLayers)
    {
        float raycastDistance = moveRange;
        float backstepDistance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, ~ignoreLayers);

        if (hit.collider != null)
        {
            return hit.point - (direction * backstepDistance);
        }
        else
        {
            return transform.position + (Vector3)(direction * moveRange / 2);
        }
    }

    protected void MoveTowardsWaypoint()
    {
        if (isStopping)
        {
            HandleStopping();
        }
        else
        {
            HandleMovement();
        }
    }

    protected void HandleStopping()
    {
        stopTime -= Time.deltaTime;
        if (stopTime <= 0)
        {
            isStopping = false;
            SetNextWaypoint();
        }
    }

    protected void HandleMovement()
    {
        timeSinceLastStop += Time.deltaTime;

        if (timeSinceLastStop >= 2.0f)
        {
            canMove = true;
        }

        Vector2 moveDirection = (CurrentWaypoint - transform.position).normalized;
        moveDirection.y = 0;
        Rb.velocity = moveDirection * MoveSpeed;

        Animator.SetBool("IsWalking", MyColor != Colors.Default);

        if (CheckCliff() || Mathf.Abs(waypointDirection) < 0.2f)
        {
            SetNextWaypoint();
        }

        if (canMove && Random.value < 0.3f * Time.deltaTime)
        {
            InitiateStopping();
        }
    }

    protected void InitiateStopping()
    {
        stopTime = Random.Range(0.4f, 1.0f);
        isStopping = true;
        canMove = false;
        timeSinceLastStop = 0;

        Animator.SetBool("IsWalking", MyColor != Colors.Default && false);
    }

    public bool CheckCliff()
    {
        Quaternion rotation = IsFlip ? FlipQuaternion : Quaternion.identity;
        Vector2 raycastOrigin = transform.position + (rotation * Vector3.right * 0.7f);

        //UnityEngine.Debug.DrawRay(raycastOrigin, Vector2.down, Color.red);

        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.0f, 1 << 0);

        return hitDown.collider == null;
    }

    private void SetNextWaypoint()  
    {
        CurrentWaypoint = IsFlip ? rightWaypoint : leftWaypoint;
    }

    public bool CheckGround()
    {
        float raycastDistance = 0.7f;

        //UnityEngine.Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, 1 << 0);

        return hit.collider != null;
    }

    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;
        UpdateHPBar();
        PlayDamageEffects();
        DisplayDamageText(damage);

        if (!isKnockedBack)
        {
            ApplyKnockback(playerPos);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageEffects()
    {
        monsterSprite.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);
        this.CallOnDelay(1f, () => { monsterSprite.DOFade(1f, 0f); });
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

        OnDie?.Invoke();

        monsterSprite.DOFade(0, 2.5f);
        hpBarBG.DOFade(0, 2f);
        Destroy(gameObject, 2.5f);
    }

    protected IEnumerator Electrocuted()
    {
        enabled = false;
        Animator.speed = 0f;

        spark = ObjectPoolManager.Instance.GetGo("Spark");
        spark.transform.SetParent(transform);
        spark.transform.localPosition = Vector3.zero;

        StartCoroutine(ShakeMonster());
        yield return new WaitForSeconds(2.0f);

        enabled = true;
        Animator.speed = 1f;
    }

    protected IEnumerator ShakeMonster()
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

    protected void OnTriggerEnter2D(Collider2D other)
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

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (!canTakeDamage_RangeAttack || IsDie) return;

        canTakeDamage_RangeAttack = false;
        if (collision.CompareTag("WeaponYellow"))
        {
            TakeDamage(15, collision.transform.position);
            StartCoroutine(Electrocuted());
        }
        else if (collision.CompareTag("WeaponOrange"))
        {
            TakeDamage(25, collision.transform.position);
        }

        this.CallOnDelay(0.5f, () => { canTakeDamage_RangeAttack = true; });
    }

    private void ApplyKnockbackForce(Vector2 direction, float force, float duration)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        float timer = 0f;
        isKnockedBack = true;

        while (timer < duration)
        {
            Rb.AddForce(force * Time.fixedDeltaTime * direction, ForceMode2D.Impulse);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.8f);
        isKnockedBack = false;
    }

    private void OnDieByGreenPlayer()
    {
        if (GameManager.Instance.playerColor == Colors.Green)
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
        hpBar.fillAmount = (float)currentHealth / maxHealth;
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

    public virtual void Attack() {}

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}