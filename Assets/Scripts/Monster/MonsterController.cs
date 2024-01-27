using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    private bool waitforAttack = true;
    private bool canAttack = true;
    private bool canWalk = true;

    private float m_JumpForce; // 없애고 싶다
    public int m_damage; // 없애고 싶다

    // Random move
    private Vector3 waypoint_L; // 좌측 목적지
    private Vector3 waypoint_R; // 우측 목적지
    private Vector3 currentWaypoint; // 현재 목적지
    public float moveRange = 20.0f; // 움직임 범위
    private float stopTime = 0; // 정지할 시간
    private float timeSinceLastStop = 0; // 마지막으로 정지한 후 경과한 시간
    private bool isStopping = false; // 정지 중인지 여부
    private bool canMove = true; // 움직일 수 있는지 여부
    private int direction = -1; // 초기 방향 설정 (1이면 오른쪽, -1이면 왼쪽)

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
        float distanceY = player.position.y - transform.position.y;

        if (myColor == Colors.def) // Default
        {
            if (currentWaypoint != null)
            {
                MoveTowardsWaypoint();
            }
        }

        else  if (myColor == Colors.red) // Red
        {
            if (player != null && canWalk)
            {
                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f && distanceY < 0.1f)
                {
                    m_sprite.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 0.1f)
                {
                    m_sprite.flipX = true;
                }

                //// �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                //if (distance < 0f && distanceY < 2f)
                //{
                //    m_sprite.flipX = false;
                //}
                //// �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                //else if (distance > 0f && distanceY < 2f)
                //{
                //    m_sprite.flipX = true;
                //}
            }

            //1. ���� ���� �ȿ� ���� ��
            if(distanceToPlayer <= attackRange)
            {
                // ���� ��Ÿ���� ���� ������ �����ϸ� -> attack
                if (canAttack && (distanceY > 0f)) // change
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    canAttack = false;
                    waitforAttack = false;
                    color.Attack(this);
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    StartCoroutine(AttackCooldown());
                    StartCoroutine(WalkCooldown());
                }
                //���ݹ������� ������ + ���� ��Ÿ���� �� �� ���Ƽ� �׳� �ȱ⸸ ������ ������ ��
                else if (waitforAttack) //walk
                {
                    if (canWalk)
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                        Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                        rb.velocity = moveDirection * moveSpeed;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
            }

            //2. ���� ���� �ȿ� ���� �� -> walk
            else if (distanceToPlayer <= detectionRange && (distanceY > 0f))
            {
                if (canWalk && (distanceY > 0f))// change
                {
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                    Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                    rb.velocity = moveDirection * moveSpeed;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }

            else //������ ���� ���� ���϶�
            {
                gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
            }
        }
        else if (myColor == Colors.blue) // Blue
        {
            if (player != null)
            {
                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f && distanceY < 2f)
                {
                    m_sprite.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 2f)
                {
                    m_sprite.flipX = true;
                }
            }

            // �÷��̾ ���� ���� �ȿ� �ְ� ���� ��ٿ��� �������� ���� ����
            if (distanceToPlayer <= attackRange && canAttack)
            {
                color.Attack(this);
                //Debug.Log("attack");
                UpdateCanAttack2();
            }
            // �÷��̾ ���� ���� �ȿ� ������ �÷��̾ ���� �̵�
            else if (distanceToPlayer <= detectionRange)
            {
                gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
            else
            {
                // ���� ������ ��� ��� �̵� ����
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
            }
        }

        else if (myColor == Colors.yellow) // Yellow
        {
            if (player != null)
            {
                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f && distanceY < 2f)
                {
                    m_sprite.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 2f)
                {
                    m_sprite.flipX = true;
                }
            }

            //1. ���� ���� �ȿ� ���� ��
            if (distanceToPlayer <= attackRange)
            {
                // ���� ��Ÿ���� ���� ������ �����ϸ� -> attack
                if (canAttack)
                {
                    canAttack = false;
                    waitforAttack = false; // �ʿ� x
                    color.Attack(this);
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    StartCoroutine(AttackCooldown());
                }
                //���ݹ������� ������ + ���� ��Ÿ���� �� �� ���Ƽ� �׳� �ȱ⸸ ������ ������ ��
                else //walk
                {
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                    gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                    Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                    rb.velocity = moveDirection * moveSpeed;
                }
            }

            //2. ���� ���� �ȿ� ���� �� -> walk
            else if (distanceToPlayer <= detectionRange)
            {
                //Debug.Log("�Ÿ�");
                gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }

            else //������ ���� ���� ���϶�
            {
                gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
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
                    stopTime = Random.Range(0.2f, 1.0f);
                    isStopping = true;
                    canMove = false;
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
        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.0f, LayerMask.GetMask("Default"));

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

    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;
        // Debug.Log(currentHealth);

        //체력바 업데이트
        UpdateHPBar();

        // �˹�
        Vector2 damageDir = new Vector3(transform.position.x - playerPos.x, 0, 0).normalized * 40f;
        rb.velocity = Vector2.zero;
        damageDir += new Vector2(0, 10).normalized * 25f;
        UpdateAttacked();
        rb.AddForce(damageDir * 5);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // ���̸� ���� ���� ����߸���
      //  if (itemPrefab != null)
       // {
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
      //  }

        // ������ ���� �˾Ƽ� �������� ������ ����
         Destroy(gameObject, 0.5f);
    }

    IEnumerator AttackCooldown()
    {
        //2�� �ִٰ� ������ ���߰�
        yield return new WaitForSeconds(2.0f);
        moveSpeed = 1.5f;
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
        gameObject.GetComponent<Animator>().SetTrigger("StopAttack");

        //���⿡ ���� �ɾƾ� �Ѵ�.
        waitforAttack = true;

        //2�� �� �ִٰ� ���� �ٽ� �� �� �ֵ���
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
        waitforAttack = false;

    }
    IEnumerator AttackCooldown2()
    {
        canAttack = false;
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
    }
    IEnumerator WalkCooldown()
    {
        canWalk = false;
        yield return new WaitForSeconds(2.0f);
        canWalk = true;
    }

    IEnumerator Attacked()
    {
        gameObject.GetComponent<MonsterController>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        gameObject.GetComponent<MonsterController>().enabled = true;
    }

    public void UpdateCanAttack()
    {
        StartCoroutine(AttackCooldown());
    }
    public void UpdateCanAttack2()
    {
        StartCoroutine(AttackCooldown2());
    }
    public void UpdateCanWalk()
    {
        StartCoroutine(WalkCooldown());
    }
    public void UpdateAttacked()
    {
        StartCoroutine(Attacked());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.tag);
        if (other.tag == "Weapon")
        {
            //Debug.Log(123);
            TakeDamage(other.GetComponentInParent<PlayerController>().damage, other.transform.position);
        }
        else if (other.gameObject.tag == "WeaponB")
        {
            //GameObject Hit = Instantiate(Resources.Load("Black_Hit"), other.transform.position, Quaternion.identity) as GameObject;
            //Destroy(Hit, 0.1f);
            TakeDamage(100, other.transform.position);
            //Destroy(other.gameObject, 0.1f);
        }
    }

    
    private void OnDieByGreenPlayer()
    {
        //Debug.Log("0000");
        if (GameManager.Instance.playerColor == Colors.green)
        {
            //Debug.Log("1111");
            Instantiate(Resources.Load("ETC/Leaf"), transform.position, Quaternion.identity);
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
