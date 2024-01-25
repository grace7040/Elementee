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
    public int currentHealth;

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
    private bool canWalk = true;

    public delegate void Del();
    public Del OnDie = null;

    // HP Bar ����
    private Image hpBar;
    private float hpBarMAX;


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
                    monsterSpriteRenderer.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 0.1f)
                {
                    monsterSpriteRenderer.flipX = true;
                }

                //// �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                //if (distance < 0f && distanceY < 2f)
                //{
                //    monsterSpriteRenderer.flipX = false;
                //}
                //// �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                //else if (distance > 0f && distanceY < 2f)
                //{
                //    monsterSpriteRenderer.flipX = true;
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
                    monsterSpriteRenderer.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 2f)
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

        else if (myColor == Colors.yellow) // Yellow
        {
            if (player != null)
            {
                // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
                if (distance < 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = false;
                }
                // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
                else if (distance > 0f && distanceY < 2f)
                {
                    monsterSpriteRenderer.flipX = true;
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
