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

    private void Awake()
    {
        SetColor();
    }
    void SetColor()
    {
        switch (color)
        {
            case M_DefaultColor:
                Color = new M_DefaultColor();
                myColor = Colors.def;
                break;

            case M_RedColor:
                Color = new M_RedColor();
                myColor = Colors.red;
                break;

            case M_BlueColor:
                Color = new M_BlueColor();
                myColor = Colors.blue;
                break;

            case M_YellowColor:
                Color = new M_YellowColor();
                myColor = Colors.yellow;
                break;
        }
    }
    void Start()
    {
        //Color = new M_RedColor();
        //Color = new M_BlueColor();
        //Color = new M_YellowColor();
        Color = new M_DefaultColor();

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

        if (Color.M_damage == 5) // Default
        {
            if (currentWaypoint != null)
            {
                MoveTowardsWaypoint();
            }
        }

        else  if (Color.M_damage == 20) // Red, Yellow
        {
            if (player != null)
            {
                // 몬스터와 플레이어의 위치 차이를 계산합니다.
                float distance = player.position.x - transform.position.x;

                // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
                if (distance < 0f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
                else if (distance > 0f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            if (distanceToPlayer <= attackRange && canAttack)
            {
                Debug.Log("Attack");
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                color.Attack(this);
                UpdateCanAttack();
            }
            // 플레이어가 감지 범위 안에 있으면 플레이어를 향해 이동
            else if (distanceToPlayer <= detectionRange)
            {
                gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
                Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
            else
            {
                // 감지 범위를 벗어난 경우 이동 중지
                gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
                gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                rb.velocity = Vector2.zero;
            }
        }
        else if (Color.M_damage == 10) // Blue
        {
            if (player != null)
            {
                // 몬스터와 플레이어의 위치 차이를 계산합니다.
                float distance = player.position.x - transform.position.x;

                // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
                if (distance < 0f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
                else if (distance > 0f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            // 플레이어가 공격 범위 안에 있고 공격 쿨다운이 끝났으면 공격 실행
            if (distanceToPlayer <= attackRange && canAttack)
            {
                color.Attack(this);
                //Debug.Log("attack");
                UpdateCanAttack();
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
        rb.AddForce(damageDir * 50);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 죽이면 색깔 물통 떨어뜨리기
        if (itemPrefab != null)
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
        }
        Destroy(gameObject);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
    }

    public void UpdateCanAttack()
    {
        StartCoroutine(AttackCooldown());
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
            //Debug.Log(123);
            TakeDamage(100, other.transform.position);
            Destroy(other.gameObject, 0.1f);
        }
    }
}
