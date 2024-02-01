using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class MonsterController : MonoBehaviour
{
    #region variables
    private M_IColorState color;
    public M_IColorState Color
    {
        get { return color; }
        set
        {
            color = value;
            m_JumpForce = value.M_JumpForce;
            m_damage = value.M_damage;
            maxHealth = value.M_health;
        }
    }
    public Colors myColor;

    private SpriteRenderer m_sprite;
    private Transform player;
    private Rigidbody2D rb;

    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    // Cooltime
    private bool canAttack = true;

    // update의 start 역할
    private bool Isfirst = true;

    private float m_JumpForce; // 없애고 싶다
    [HideInInspector]
    public int m_damage; // 없애고 싶다

    // Random move
    private Vector3 waypoint_L; // 좌측 목적지
    private Vector3 waypoint_R; // 우측 목적지
    private Vector3 currentWaypoint; // 현재 목적지
    public float moveRange = 10.0f; // 움직임 범위
    private float stopTime = 0; // 정지할 시간
    private float timeSinceLastStop = 0; // 마지막으로 정지한 후 경과한 시간
    private bool isStopping = false; // 정지 중인지 여부
    private bool canMove = true; // 움직일 수 있는지 여부
    private int direction = -1; // 초기 방향 설정 (1이면 오른쪽, -1이면 왼쪽)
    private float timer = 0.0f; // 타이머 변수
    private float interval = 5.0f; // 호출 간격

    // 넉백
    private bool isKnockedBack = false;

    // Die
    public delegate void Del();
    public Del OnDie = null;

    // HP Bar
    private Image hpBar;
    private float hpBarMAX;
    #endregion

    private void Awake()
    {
        SetColor();
    }

    private void SetColor()
    {
        switch (myColor)
        {
            case Colors.def:
                Color = new M_DefaultColor();
                break;

            case Colors.red:
                Color = new M_RedColor();
                break;

            case Colors.blue:
                Color = new M_BlueColor();
                break;

            case Colors.yellow:
                Color = new M_YellowColor();
                break;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        m_sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // waypoint 초기화
        SetWaypoints();
        currentWaypoint = waypoint_L;

        // 체력바
        hpBar = transform.Find("HPBar").GetChild(1).gameObject.GetComponent<Image>();
        hpBarMAX = hpBar.gameObject.GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distance = player.position.x - transform.position.x;

        if (myColor == Colors.def) // Default
        {
            // 일정 간격마다 waypoint 재탐색
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                SetWaypoints();
                timer = 0.0f;
            }

            if (isKnockedBack) { }
            else
            {
                if (currentWaypoint != null)
                {
                    MoveTowardsWaypoint();
                }
            }
        }

        else  if (myColor == Colors.red) // Red
        {
            if (distanceToPlayer <= detectionRange)
            {
                Isfirst = true;

                // Flip
                if (distance < -0.1f)
                {
                    m_sprite.flipX = false;
                }
                else if (distance > 0.1f)
                {
                    m_sprite.flipX = true;
                }

                // Attack
                if (distanceToPlayer <= attackRange)
                {
                    if (!CheckGround()) { }
                    else
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                        rb.velocity = Vector2.zero;

                        if (canAttack)
                        {
                            gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
                            color.Attack(this);
                            StartCoroutine(AttackCooldown());
                        }
                    }
                }
                else
                {
                    if (!CheckGround())
                    {
                        rb.velocity += Vector2.down * 1f * Time.deltaTime;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            // Move
                            gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                            Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                            rb.velocity = moveDirection * moveSpeed;
                        }
                    }
                }
            }
            else
            {
                if (canAttack)
                {
                    if (Isfirst)
                    {
                        SetWaypoints();
                        currentWaypoint = waypoint_L;
                        m_sprite.flipX = false;
                        Isfirst = false;
                    }

                    timer += Time.deltaTime;

                    if (timer >= interval)
                    {
                        SetWaypoints();
                        timer = 0.0f;
                    }

                    if (isKnockedBack) { }
                    else
                    {
                        if (currentWaypoint != null)
                        {
                            MoveTowardsWaypoint();
                        }
                    }
                }
            }
        }

        else if (myColor == Colors.blue) // Blue
        {
            if (distanceToPlayer <= detectionRange)
            {
                Isfirst = true;

                // Flip
                if (distance < 0f)
                {
                    m_sprite.flipX = false;
                }
                else if (distance > 0f)
                {
                    m_sprite.flipX = true;
                }

                // Attack
                if (distanceToPlayer <= attackRange)
                {
                    if (!CheckGround()) { }
                    else
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                        rb.velocity = Vector2.zero;

                        if (canAttack)
                        {
                            gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
                            color.Attack(this);
                            StartCoroutine(AttackCooldown());
                        }
                    }
                }
                else
                {
                    if (!CheckGround())
                    {
                        rb.velocity += Vector2.down * 1f * Time.deltaTime;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            // Move
                            gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                            Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                            rb.velocity = moveDirection * moveSpeed;
                        }
                    }
                }
            }
            else
            {
                if (canAttack)
                {
                    if (Isfirst)
                    {
                        SetWaypoints();
                        currentWaypoint = waypoint_L;
                        m_sprite.flipX = false;
                        Isfirst = false;
                    }

                    timer += Time.deltaTime;

                    if (timer >= interval)
                    {
                        SetWaypoints();
                        timer = 0.0f;
                    }

                    if (isKnockedBack) { }
                    else
                    {
                        if (currentWaypoint != null)
                        {
                            MoveTowardsWaypoint();
                        }
                    }
                }
            }
        }

        else if (myColor == Colors.yellow) // Yellow
        {
            if (distanceToPlayer <= detectionRange)
            {
                Isfirst = true;

                // Flip
                if (distance < -0.1f)
                {
                    m_sprite.flipX = false;
                }
                else if (distance > 0.1f)
                {
                    m_sprite.flipX = true;
                }

                // Attack
                if (distanceToPlayer <= attackRange)
                {
                    if (!CheckGround()) { }
                    else
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);

                        if (canAttack)
                        {
                            // 잠시 멈췄다가 돌진
                            StartCoroutine(ChargeAfterDelay());
                            color.Attack(this);
                        }
                    }
                }
                else
                {
                    if (!CheckGround())
                    {
                        rb.velocity += Vector2.down * 1f * Time.deltaTime;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            // Move
                            gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                            Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                            rb.velocity = moveDirection * moveSpeed;
                        }
                    }
                }
            }
            else
            {
                if (canAttack)
                {
                    if (Isfirst)
                    {
                        SetWaypoints();
                        currentWaypoint = waypoint_L;
                        m_sprite.flipX = false;
                        Isfirst = false;
                    }

                    timer += Time.deltaTime;

                    if (timer >= interval)
                    {
                        SetWaypoints();
                        timer = 0.0f;
                    }

                    if (isKnockedBack) { }
                    else
                    {
                        if (currentWaypoint != null)
                        {
                            MoveTowardsWaypoint();
                        }
                    }
                }
            }
        }
    }
    private void SetWaypoints()
    {
        // 무시할 Layer 설정
        LayerMask ignoreLayers = LayerMask.GetMask("m_self", "Player", "DectectArea", "TransparentFX");

        float raycastDistance = moveRange;
        float backstepDistance = 1.0f; // Collider 크기 반영

        // 좌측 가장 가까운 물체 찾기
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, ~ignoreLayers);
        if (hitLeft.collider != null)
        {
            waypoint_L = hitLeft.point - (Vector2.left * backstepDistance);
        }
        else
        {
            // 아무 것도 감지되지 않을 시, moveRange의 끝 지점으로 지정
            waypoint_L = new Vector3(transform.position.x - moveRange / 2, transform.position.y, transform.position.z);
        }

        // 우측 가장 가까운 물체 찾기
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, ~ignoreLayers);
        if (hitRight.collider != null)
        {
            waypoint_R = hitRight.point - (Vector2.right * backstepDistance);
        }
        else
        {
            // 아무 것도 감지되지 않을 시, moveRange의 끝 지점으로 지정
            waypoint_R = new Vector3(transform.position.x + moveRange / 2, transform.position.y, transform.position.z);
        }
    }

    private void MoveTowardsWaypoint()
    {
        if (canMove)
        {
            if (isStopping)
            {
                // 정지 중일 때
                stopTime -= Time.deltaTime;
                if (stopTime <= 0)
                {
                    isStopping = false;
                    SetNextWaypoint();
                }
            }
            else
            {
                // 이동 중일 때
                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);

                if (myColor != Colors.def)
                {
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                }

                // 낭떠러지 여부 확인
                if (CheckCliff())
                {
                    SetNextWaypoint();
                }

                // 목적지 도착 여부 확인
                if (Vector2.Distance(transform.position, currentWaypoint) < 0.1f)
                {
                    SetNextWaypoint();
                }

                // 일정 시간마다 정지할지 결정
                if (Random.value < 0.3 * Time.deltaTime)
                {
                    // 정지할 시간을 랜덤으로 설정
                    stopTime = Random.Range(0.1f, 0.3f); // TOCHANGE
                    isStopping = true;
                    canMove = false;
                    if (myColor != Colors.def)
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    }
                }
            }
        }
        else
        {
            // 정지 후 대기 중일 때
            timeSinceLastStop += Time.deltaTime;
            if (timeSinceLastStop >= 2.0f)
            {
                canMove = true;
                timeSinceLastStop = 0;
            }
        }
    }

    private bool CheckCliff()
    {
        // Raycast를 사용하여 앞쪽으로 바닥 감지
        Vector2 raycastOrigin = transform.position + (Vector3.right * direction * 1.0f);
        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.0f, LayerMask.GetMask("Default")); // To change

        // 바닥이 감지되지 않으면 낭떠러지로 판단
        return hitDown.collider == null;
    }

    private void SetNextWaypoint()
    {
        if (!m_sprite.flipX)
        {
            m_sprite.flipX = true;
            currentWaypoint = waypoint_R;
            direction *= -1;
        }
        else
        {
            m_sprite.flipX = false;
            currentWaypoint = waypoint_L;
            direction *= -1;
        }
    }

    private bool CheckGround()
    {
        float raycastDistance = 0.8f; // 바닥과의 간격 설정
        LayerMask groundLayer = LayerMask.GetMask("Default"); // To change

        // 몬스터 아래에 레이캐스트를 쏘아 발판이 있는지 확인
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        // Debug.DrawRay(transform.position, Vector2.down * raycastDistance, UnityEngine.Color.red);

        // 발판이 없다면 추락
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }

    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;
        Debug.Log($"몬스터 대미지: {damage}");

        //체력바 업데이트
        UpdateHPBar();

        //피격 이팩트 추가
        m_sprite.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);

        if (!isKnockedBack)
        {
            rb.velocity = Vector2.zero;

            // 피격 방향 계산
            Vector2 damageDir = new Vector2(transform.position.x - playerPos.x, 0).normalized * 2f;

            // 넉백 윗 방향 추가
            damageDir += new Vector2(0, 1).normalized * 2f;

            // 넉백 방향으로 힘을 일정한 시간 동안 부여
            ApplyKnockbackForce(damageDir, 10f, 0.2f);

            // 피격 시 실행할 함수
            // UpdateAttacked();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        switch (myColor)
        {
            case Colors.def:
                break;
            case Colors.red:
                ColorManager.Instance.HasRed = true;
                break;
            case Colors.blue:
                ColorManager.Instance.HasBlue = true;
                break;
            case Colors.yellow:
                ColorManager.Instance.HasYellow = true;
                break;
        }

        Destroy(gameObject, 0.5f);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        rb.velocity = Vector2.zero;
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
        canAttack = true;
    }

    IEnumerator ChargeAfterDelay()
    {
        canAttack = false;
        rb.velocity = Vector2.zero;
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(ChargeForDuration(0.4f));
        //gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
    }
    IEnumerator ChargeForDuration(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            Vector2 direction = (player.position - transform.position);
            direction.y = 0;
            direction.Normalize();
            Vector2 movement = direction * 7f * Time.deltaTime;
            transform.Translate(movement);

            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            TakeDamage(other.GetComponentInParent<PlayerController>().damage, other.transform.position);
        }
        else if (other.gameObject.tag == "WeaponThrow")
        {
            TakeDamage(35, other.transform.position);
        }
        else if (other.gameObject.tag == "WeaponB")
        {
            TakeDamage(100, other.transform.position);
        }
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
            // deltaTime 대신 Time.fixedDeltaTime 사용
            rb.AddForce(direction * force * Time.fixedDeltaTime, ForceMode2D.Impulse);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.8f);
        isKnockedBack = false;
    }

    private void OnDieByGreenPlayer()
    {
        if (GameManager.Instance.playerColor == Colors.green)
        {
            Instantiate(Resources.Load("Monster/Leaf"), transform.position, Quaternion.identity);
        }
            
    }

    // 체력바 업데이트
    private void UpdateHPBar()
    {
        hpBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void SetOnDieByGreenPlayer()
    {
        OnDie = OnDieByGreenPlayer;
    }

    private void OnDestroy()
    {
        OnDie?.Invoke();

    }
}