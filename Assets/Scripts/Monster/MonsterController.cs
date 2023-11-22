using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
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
    public int maxHealth = 100;
    private int currentHealth;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody2D rb;

    public GameObject itemPrefab; // 떨어뜨릴 아이템 프리팹

    private float m_JumpForce;
    public int m_damage;

    public bool canAttack = true;

    private SpriteRenderer monsterSpriteRenderer;

    public Transform[] waypoints; // AI가 이동할 Waypoint들의 배열

    private int currentWaypointIndex = 0;
    private Transform currentWaypoint;

    //public Animator animator;

    private bool waitforAttack = true;
    private bool canWalk = true;

    public delegate void Del();
    public Del OnDie = null;
    private void Awake()
    {
        SetColor();
    }
    void SetColor()
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
    void Start()
    {
        //Color = new M_RedColor();
        //Color = new M_BlueColor();
        //Color = new M_YellowColor();
        //Color = new M_RedColor();

        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; // "Player" 태그를 가진 오브젝트를 플레이어로 설정
        monsterSpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (waypoints.Length > 0)
        {
            currentWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
                // 몬스터와 플레이어의 위치 차이를 계산합니다.
                float distance = player.position.x - transform.position.x;
                float distanceY = player.position.y - transform.position.y;

                // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
                if (distance < 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
                else if (distance > 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            //1. 공격 범위 안에 있을 때
            if(distanceToPlayer <= attackRange)
            {
                // 공격 쿨타임이 차서 공격이 가능하면 -> attack
                if (canAttack)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    canAttack = false;
                    waitforAttack = false;
                    color.Attack(this);
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    StartCoroutine(AttackCooldown());
                    StartCoroutine(WalkCooldown());
                }
                //공격범위에는 있지만 + 공격 쿨타임이 다 안 돌아서 그냥 걷기만 했으면 좋겠을 때
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

            //2. 감지 범위 안에 있을 때 -> walk
            else if (distanceToPlayer <= detectionRange)
            {
                if (canWalk)
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

            else //완전히 공격 범위 밖일때
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
                // 몬스터와 플레이어의 위치 차이를 계산합니다.
                float distance = player.position.x - transform.position.x;
                float distanceY = player.position.y - transform.position.y;

                // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
                if (distance < 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
                else if (distance > 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            // 플레이어가 공격 범위 안에 있고 공격 쿨다운이 끝났으면 공격 실행
            if (distanceToPlayer <= attackRange && canAttack)
            {
                color.Attack(this);
                //Debug.Log("attack");
                UpdateCanAttack2();
            }
            // 플레이어가 감지 범위 안에 있으면 플레이어를 향해 이동
            else if (distanceToPlayer <= detectionRange)
            {
                gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
            else
            {
                // 감지 범위를 벗어난 경우 이동 중지
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
            }
        }

        else if (myColor == Colors.yellow) // Yellow
        {
            if (player != null)
            {
                // 몬스터와 플레이어의 위치 차이를 계산합니다.
                float distance = player.position.x - transform.position.x;
                float distanceY = player.position.y - transform.position.y;

                // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
                if (distance < 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
                else if (distance > 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            //1. 공격 범위 안에 있을 때
            if (distanceToPlayer <= attackRange)
            {
                // 공격 쿨타임이 차서 공격이 가능하면 -> attack
                if (canAttack)
                {
                    canAttack = false;
                    waitforAttack = false; // 필요 x
                    color.Attack(this);
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    StartCoroutine(AttackCooldown());
                }
                //공격범위에는 있지만 + 공격 쿨타임이 다 안 돌아서 그냥 걷기만 했으면 좋겠을 때
                else //walk
                {
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                    gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                    Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                    rb.velocity = moveDirection * moveSpeed;
                }
            }

            //2. 감지 범위 안에 있을 때 -> walk
            else if (distanceToPlayer <= detectionRange)
            {
                Debug.Log("거리");
                gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }

            else //완전히 공격 범위 밖일때
            {
                gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void MoveTowardsWaypoint()
    {
        // 현재 Waypoint로 이동
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        // 만약 AI가 현재 Waypoint에 도착했다면 다음 Waypoint로 변경
        if (Vector2.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        if (!monsterSpriteRenderer.flipX)
        {
            monsterSpriteRenderer.flipX = true;
        }
        else
        {
            monsterSpriteRenderer.flipX = false;
        }

        // 다음 Waypoint을 설정하고, 배열의 끝에 도달하면 처음 Waypoint으로 돌아감
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        // 넉백
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
        // 죽이면 색깔 물통 떨어뜨리기
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

        // 블랙일 때는 알아서 없어지기 때문에 생략
         Destroy(gameObject, 0.5f);
    }

    IEnumerator AttackCooldown()
    {
        //2초 있다가 공격은 멈추고
        yield return new WaitForSeconds(2.0f);
        moveSpeed = 1.5f;
        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
        gameObject.GetComponent<Animator>().SetTrigger("StopAttack");

        //여기에 지금 걸아야 한다.
        waitforAttack = true;

        //2초 더 있다가 공격 다시 할 수 있도록
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
        Debug.Log(other.gameObject.tag);
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
            Instantiate(Resources.Load("Leaf"), transform.position, Quaternion.identity);
        }
            
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
