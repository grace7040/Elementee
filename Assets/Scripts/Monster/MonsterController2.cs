//using GooglePlayGames.BasicApi;
//using System.Collections;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Threading;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.UI;
//using DG.Tweening;
//using TMPro;
//using UnityEngine.EventSystems;
//using static UnityEditor.FilePathAttribute;

//public class MonsterController2 : MonoBehaviour
//{
//    #region variables

//    public Monster monsterData;
//    public Colors myColor;

//    public bool isDie = false;

//    protected SpriteRenderer m_sprite;
//    static protected Transform player;
//    protected Rigidbody2D rb;

//    private int maxHealth;
//    protected int currentHealth;
//    protected float moveSpeed = 3f;
//    protected float detectionRange = 10f;
//    protected float attackRange;

//    // Cooltime
//    protected bool canAttack = true;
//    protected bool canTakeDamage_RangeAttack = true;

//    // update의 start 역할
//    protected bool Isfirst = true;

//    [HideInInspector]
//    public int damage;

//    // Random move
//    protected Vector3 waypoint_L; // 좌측 목적지
//    protected Vector3 waypoint_R; // 우측 목적지
//    protected Vector3 currentWaypoint; // 현재 목적지
//    protected float moveRange = 50.0f; // 움직임 범위
//    protected float stopTime = 0; // 정지할 시간
//    protected float timeSinceLastStop = 0; // 마지막으로 정지한 후 경과한 시간
//    protected bool isStopping = false; // 정지 중인지 여부
//    protected bool canMove = true; // 움직일 수 있는지 여부
//    protected float timer = 0.0f; // 타이머 변수
//    protected float interval = 5.0f; // 호출 간격

//    // 넉백
//    protected bool isKnockedBack = false;

//    // Die
//    public delegate void Del();
//    public Del OnDie = null;

//    // HP Bar
//    protected Image hpBar;
//    protected Image hpBarBG;
//    protected float hpBarMAX;

//    protected float waypointDirection;
//    protected float distanceX;
//    protected float distanceY;
//    public Animator animator;

//    Quaternion flipQuaternion = Quaternion.Euler(new Vector3(0, 180, 0));
//    public Transform monsterBody;

//    protected bool isFlip = false;
//    protected bool canflip = true;
//    protected bool isGrounded = true;
//    #endregion

//    protected Vector2 dir;
//    protected GameObject playerobj;

//    GameObject Spark;

//    protected void Awake()
//    {
//        damage = monsterData.Damage;
//        maxHealth = monsterData.Health;
//        myColor = monsterData.MyColor;
//        attackRange = monsterData.AttackRange;

//        currentHealth = maxHealth;
//    }

//    protected void Start()
//    {
//        if(player == null)
//        {
//            playerobj = GameManager.Instance.player;
//            player = playerobj.transform;
//        }

//        m_sprite = GetComponentInChildren<SpriteRenderer>();
//        rb = GetComponent<Rigidbody2D>();

//        // waypoint 초기화
//        SetWaypoints();
//        currentWaypoint = waypoint_L;

//        // 체력바
//        hpBar = transform.Find("HPBar").GetChild(1).gameObject.GetComponent<Image>();
//        hpBarMAX = hpBar.gameObject.GetComponent<RectTransform>().rect.width;
//        hpBarBG = transform.Find("HPBar").GetChild(0).gameObject.GetComponent<Image>();

//    }

//    protected void Update()
//    {
//        if (isDie) return;

//        if (playerobj != null) isGrounded = playerobj.GetComponent<PlayerController>().m_Grounded;

//        waypointDirection = currentWaypoint.x - transform.position.x;
//        distanceX = Mathf.Abs(transform.position.x - player.position.x);
//        distanceY = Mathf.Abs(transform.position.y - player.position.y);

//        isFlip = waypointDirection < 0f;
//        //print(waypointDirection);
        
//        if (myColor == Colors.Yellow)
//        {
//            if (canflip) monsterBody.rotation = isFlip ? Quaternion.identity : flipQuaternion;
//        }
//        else
//        {
//            monsterBody.rotation = isFlip ? Quaternion.identity : flipQuaternion;
//        }
//    }

//    protected void Move()
//    {
//        timer += Time.deltaTime;

//        if (timer >= interval)
//        {
//            SetWaypoints();
//            timer = 0.0f;
//        }

//        if (currentWaypoint != null && !isKnockedBack)
//        {
//            MoveTowardsWaypoint();
//        }
//    }
//    protected void SetWaypoints()
//    {
//        // 무시할 Layer 설정
//        LayerMask ignoreLayers = LayerMask.GetMask("Monster", "Player", "TransparentFX", "Coins");

//        float raycastDistance = moveRange;
//        float backstepDistance = 1.0f; // Collider 크기 반영

//        // 좌측 가장 가까운 물체 찾기
//        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, ~ignoreLayers);
//        //UnityEngine.Debug.DrawRay(transform.position, Vector2.down * raycastDistance, UnityEngine.Color.red);

//        if (hitLeft.collider != null)
//        {
//            waypoint_L = hitLeft.point - (Vector2.left * backstepDistance);
//        }
//        else
//        {
//            // 아무 것도 감지되지 않을 시, moveRange의 끝 지점으로 지정
//            waypoint_L = new Vector3(transform.position.x - moveRange / 2, transform.position.y, transform.position.z);
//        }

//        // 우측 가장 가까운 물체 찾기
//        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, ~ignoreLayers);
//        if (hitRight.collider != null)
//        {
//            waypoint_R = hitRight.point - (Vector2.right * backstepDistance);
//        }
//        else
//        {
//            // 아무 것도 감지되지 않을 시, moveRange의 끝 지점으로 지정
//            waypoint_R = new Vector3(transform.position.x + moveRange / 2, transform.position.y, transform.position.z);
//        }
//    }

//    protected void MoveTowardsWaypoint()
//    {
//        if (isStopping)
//        {
//            // 정지 중일 때
//            stopTime -= Time.deltaTime;
//            if (stopTime <= 0)
//            {
//                isStopping = false;
//                SetNextWaypoint();
//            }
//        }
//        else
//        {
//            timeSinceLastStop += Time.deltaTime;
//            if (timeSinceLastStop >= 2.0f)
//            {
//                canMove = true;
//            }

//            // 이동 중일 때
//            Vector2 moveDirection = new Vector2(currentWaypoint.x - transform.position.x, 0).normalized;
//            rb.velocity = moveDirection * moveSpeed;

//            if (myColor != Colors.Default)
//            {
//                animator.SetBool("IsWalking", true);
//            }

//            // 낭떠러지 여부 확인
//            if (CheckCliff())
//            {
//                SetNextWaypoint();
//            }

//            // 목적지 도착 여부 확인
//            if (Mathf.Abs(waypointDirection) < 0.2f)
//            {
//                SetNextWaypoint();
//            }

//            if (canMove)
//            {
//                // 일정 시간마다 정지할지 결정
//                if (Random.value < 0.3 * Time.deltaTime)
//                {
//                    // 정지할 시간을 랜덤으로 설정
//                    stopTime = Random.Range(0.4f, 1.0f); // TOCHANGE
//                    isStopping = true;
//                    canMove = false;
//                    timeSinceLastStop = 0;
//                    if (myColor != Colors.Default)
//                    {
//                        animator.SetBool("IsWalking", false);
//                    }
//                }
//            }
//        }
//    }

//    public bool CheckCliff()
//    {
//        // Raycast를 사용하여 앞쪽으로 바닥 감지
//        Quaternion rotation = isFlip ? flipQuaternion : Quaternion.identity;
//        Vector2 raycastOrigin = transform.position + (rotation * Vector3.right);
//        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.0f, 1 << 0); // To change
//        // UnityEngine.Debug.DrawRay(raycastOrigin, Vector2.down * 1.0f, UnityEngine.Color.red);

//        // 바닥이 감지되지 않으면 낭떠러지로 판단
//        return hitDown.collider == null;
//    }

//    private void SetNextWaypoint()
//    {
//        if (isFlip) currentWaypoint = waypoint_R;
//        else currentWaypoint = waypoint_L;
//    }

//    protected bool CheckGround()
//    {
//        float raycastDistance = 1.0f; // 바닥과의 간격 설정

//        // 몬스터 아래에 레이캐스트를 쏘아 발판이 있는지 확인
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, 1 << 0);
//        //UnityEngine.Debug.DrawRay(transform.position, Vector2.down * raycastDistance, UnityEngine.Color.red);

//        // 발판이 없다면 추락
//        if (hit.collider == null)
//        {
//            return false;
//        }
//        return true;
//    }

//    public void TakeDamage(int damage, Vector3 playerPos)
//    {
//        //Debug.Log($"Monster 대미지 : {damage}");
//        currentHealth -= damage;
        
//        //체력바 업데이트
//        UpdateHPBar();

//        //피격 이팩트 추가
//        m_sprite.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);
//        this.CallOnDelay(1f, () => { m_sprite.DOFade(1f, 0f); });
        

//        //데미지 텍스트
//        var DamageText = ObjectPoolManager.Instance.GetGo("DamageText");
//        DamageText.GetComponent<TextMeshPro>().text = damage.ToString();
//        DamageText.transform.position = transform.position;
//        DamageText.transform.SetParent(this.transform);


//        if (!isKnockedBack)
//        {
//            rb.velocity = Vector2.zero;

//            // 피격 방향 계산
//            Vector2 damageDir = new Vector2(transform.position.x - playerPos.x, 0).normalized * 2f;

//            // 넉백 윗 방향 추가
//            damageDir += new Vector2(0, 1).normalized * 2f;

//            // 넉백 방향으로 힘을 일정한 시간 동안 부여
//            ApplyKnockbackForce(damageDir, 12f, 0.3f);
//        }

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    public void Die()
//    {
//        if (isDie) return;

//        isDie = true;
//        switch (myColor)
//        {
//            case Colors.Default:
//                break;
//            case Colors.Red:
//                ColorManager.Instance.HasRed = true;
//                break;
//            case Colors.Blue:
//                ColorManager.Instance.HasBlue = true;
//                break;
//            case Colors.Yellow:
//                ColorManager.Instance.HasYellow = true;
//                break;
//        }

//        animator.enabled = false;
//        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;

//        OnDie?.Invoke();


//        // 서서히 사라지게 하기
//        m_sprite.DOFade(0, 2.5f);
//        hpBarBG.DOFade(0, 2f);
//        Destroy(gameObject, 2.5f);
//    }


//    protected IEnumerator ChargeAfterDelay()
//    {
//        canAttack = false;
//        canflip = false;
//        rb.velocity = Vector2.zero;
//        dir = new Vector2(waypointDirection, 0);
//        yield return new WaitForSeconds(1.2f);
//        animator.SetBool("IsAttacking", true);
//        StartCoroutine(ChargeForDuration(0.4f));
//    }

//    IEnumerator ChargeForDuration(float duration)
//    {
//        float startTime = Time.time;

//        dir.y = 0;
//        dir.Normalize();

//        while (Time.time - startTime < duration)
//        {
//            Vector2 movement = 7f * Time.deltaTime * dir;
//            transform.Translate(movement);

//            yield return null;
//        }
//        yield return new WaitForSeconds(1.0f);
//        animator.SetBool("IsAttacking", false);
//        canAttack = true;
//        canflip = true;
//    }

//    protected IEnumerator Electrocuted()
//    {
//        enabled = false;
//        animator.speed = 0f;

//        Spark = ObjectPoolManager.Instance.GetGo("Spark");
//        Spark.transform.SetParent(transform);
//        Spark.transform.localPosition = Vector3.zero;

//        StartCoroutine(ShakeMonster());
//        yield return new WaitForSeconds(2.0f);
//        enabled = true;
//        animator.speed = 1f;
//    }

//    protected IEnumerator ShakeMonster()
//    {
//        float shakeDuration = 1.5f;
//        float shakeIntensity = 0.05f;

//        while (shakeDuration > 0)
//        {
//            transform.position += new Vector3(Random.Range(-shakeIntensity, shakeIntensity), 0f, 0f);

//            shakeDuration -= Time.deltaTime;

//            yield return null;
//        }
//    }

//    protected void OnTriggerEnter2D(Collider2D other)
//    {
//        if (isDie) return;

//        if (other.CompareTag("Weapon"))
//        {
//            TakeDamage(other.GetComponentInParent<PlayerController>().damage, other.transform.position);
//        }
//        else if (other.gameObject.CompareTag("WeaponThrow"))
//        {
//            TakeDamage(35, other.transform.position);
//        }
//        else if (other.gameObject.CompareTag("WeaponB"))
//        {
//            TakeDamage(100, other.transform.position);
//        }
//    }

//    protected void OnTriggerStay2D(Collider2D collision)
//    {
//        if (!canTakeDamage_RangeAttack || isDie) return;

//        canTakeDamage_RangeAttack = false;
//        if (collision.CompareTag("WeaponYellow"))
//        {
//            TakeDamage(15, collision.transform.position);
//            StartCoroutine(Electrocuted());
//        }
//        else if(collision.CompareTag("WeaponOrange"))
//        {
//            TakeDamage(25, collision.transform.position);
//        }

//        this.CallOnDelay(0.5f, () => { canTakeDamage_RangeAttack = true; });
//    }

//    private void ApplyKnockbackForce(Vector2 direction, float force, float duration)
//    {
//        StartCoroutine(KnockbackCoroutine(direction, force, duration));
//    }

//    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
//    {
//        float timer = 0f;
//        isKnockedBack = true;

//        while (timer < duration)
//        {
//            rb.AddForce(force * Time.fixedDeltaTime * direction, ForceMode2D.Impulse);

//            timer += Time.fixedDeltaTime;
//            yield return new WaitForFixedUpdate();
//        }

//        yield return new WaitForSeconds(0.8f);
//        isKnockedBack = false;
//    }

//    private void OnDieByGreenPlayer()
//    {
//        if (GameManager.Instance.playerColor == Colors.Green)
//        {
//            Instantiate(Resources.Load("Monster/Leaf"), transform.position, Quaternion.identity);
//        }
//        else
//        {
//            OnDie -= OnDieByGreenPlayer;
//        }
            
//    }

//    // 체력바 업데이트
//    private void UpdateHPBar()
//    {
//        hpBar.fillAmount = (float)currentHealth / maxHealth;
//    }

//    public void SetOnDieByGreenPlayer()
//    {
//        OnDie = OnDieByGreenPlayer;
//    }
//    public void PulledByBlack()
//    {
//        animator.enabled = false;
//        isDie = true;
//        if (myColor == Colors.Yellow) GetComponent<M_Yellow>().voltObject.SetActive(false);
//        else if (myColor == Colors.Red) GetComponent<M_Red>().fireObject.SetActive(false);
//        enabled = false;
//        rb.mass = 0.0f;
//        rb.gravityScale = 0.0f;
//        GetComponent<CapsuleCollider2D>().isTrigger = true;
//    }
//}