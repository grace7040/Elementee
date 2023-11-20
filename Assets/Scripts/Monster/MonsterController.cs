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

    public GameObject itemPrefab; // ����߸� ������ ������

    private float m_JumpForce;
    public int m_damage;

    public bool canAttack = true;

    private SpriteRenderer monsterSpriteRenderer;

    public Transform[] waypoints; // AI�� �̵��� Waypoint���� �迭

    private int currentWaypointIndex = 0;
    private Transform currentWaypoint;

    //public Animator animator;

    private bool waitforAttack = true;

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
        player = GameObject.FindGameObjectWithTag("Player").transform; // "Player" �±׸� ���� ������Ʈ�� �÷��̾�� ����
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
                // ���Ϳ� �÷��̾��� ��ġ ���̸� ����մϴ�.
                float distance = player.position.x - transform.position.x;

                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f)
                {
                    monsterSpriteRenderer.flipX = true;
                }
            }

            //1. ���� ���� �ȿ� ���� ��
            if(distanceToPlayer <= attackRange)
            {
                // ���� ��Ÿ���� ���� ������ �����ϸ� -> attack
                if (canAttack)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    canAttack = false;
                    waitforAttack = false;
                    color.Attack(this);
                    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                    StartCoroutine(AttackCooldown());
                }
                //���ݹ������� ������ + ���� ��Ÿ���� �� �� ���Ƽ� �׳� �ȱ⸸ ������ ������ ��
                else if (waitforAttack) //walk
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
                Debug.Log("�Ÿ�");
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
            

            ///////// ���� �ڵ�
            //if (canAttack)
            //{
            //    if (distanceToPlayer <= attackRange)
            //    {
            //        Debug.Log("Attack");
            //        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //        canAttack = false;
            //        color.Attack(this);
            //        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            //        StartCoroutine(AttackCooldown());
            //    }
            //    else if (distanceToPlayer <= detectionRange)
            //    {
            //        Debug.Log("�Ÿ�");
            //        gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
            //        Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
            //        rb.velocity = moveDirection * moveSpeed;
            //    }
            //    else
            //    {
            //        // ���� ������ ��� ��� �̵� ����
            //        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            //        rb.velocity = Vector2.zero;
            //    }
            //}
            //else
            //{
            //    if (distanceToPlayer <= detectionRange)
            //    {
            //        Debug.Log("�Ÿ�");
            //        gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
            //        Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
            //        rb.velocity = moveDirection * moveSpeed;
            //    }
            //    else
            //    {
            //        // ���� ������ ��� ��� �̵� ����
            //        gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            //        gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            //        rb.velocity = Vector2.zero;
            //    }
            //}
            //if (distanceToPlayer <= attackRange && canAttack)
            //{
            //    Debug.Log("Attack");
            //    canAttack = false;
            //    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            //    color.Attack(this);
            //    UpdateCanAttack();
            //}
            //// �÷��̾ ���� ���� �ȿ� ������ �÷��̾ ���� �̵�
            //else if (distanceToPlayer <= detectionRange)
            //{
            //    Debug.Log("�Ÿ�");
            //    //gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            //    gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
            //    Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
            //    rb.velocity = moveDirection * moveSpeed;
            //}
            //else
            //{
            //    // ���� ������ ��� ��� �̵� ����
            //    //gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            //    gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            //    rb.velocity = Vector2.zero;
            //}
        }
        else if (Color.M_damage == 10) // Blue
        {
            if (player != null)
            {
                // ���Ϳ� �÷��̾��� ��ġ ���̸� ����մϴ�.
                float distance = player.position.x - transform.position.x;

                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f)
                {
                    monsterSpriteRenderer.flipX = true;
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
    }

    private void MoveTowardsWaypoint()
    {
        // ���� Waypoint�� �̵�
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        // ���� AI�� ���� Waypoint�� �����ߴٸ� ���� Waypoint�� ����
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

        // ���� Waypoint�� �����ϰ�, �迭�� ���� �����ϸ� ó�� Waypoint���� ���ư�
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        // �˹�
        Vector2 damageDir = new Vector3(transform.position.x - playerPos.x, 0, 0).normalized * 40f;
        rb.velocity = Vector2.zero;
        rb.AddForce(damageDir * 5);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ���̸� ���� ���� ����߸���
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
        Destroy(gameObject, 0.5f);
    }

    IEnumerator AttackCooldown()
    {
        //2�� �ִٰ� ������ ���߰�
        yield return new WaitForSeconds(2.0f);
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

    public void UpdateCanAttack()
    {
        StartCoroutine(AttackCooldown());
    }
    public void UpdateCanAttack2()
    {
        StartCoroutine(AttackCooldown2());
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

    
    private void OnDieByGreenPlayer()
    {
        Debug.Log("0000");
        if (GameManager.Instance.playerColor == Colors.green)
        {
            Debug.Log("1111");
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
