using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
                break;

            case M_RedColor:
                Color = new M_RedColor();
                break;

            case M_BlueColor:
                Color = new M_BlueColor();
                break;

            case M_YellowColor:
                Color = new M_YellowColor();
                break;
        }
    }
    void Start()
    {
        //Color = new M_RedColor();
        //Color = new M_BlueColor();
        Color = new M_YellowColor();
        //Color = new M_DefaultColor();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; // "Player" �±׸� ���� ������Ʈ�� �÷��̾�� ����
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ���� ���� �ȿ� �ְ� ���� ��ٿ��� �������� ���� ����
        if (distanceToPlayer <= attackRange && canAttack)
        {
            color.Attack(this);
            //Debug.Log("attack");
            UpdateCanAttack();
        }
        // �÷��̾ ���� ���� �ȿ� ������ �÷��̾ ���� �̵�
        else if (distanceToPlayer <= detectionRange)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed; 
        }
        else
        {
            // ���� ������ ��� ��� �̵� ����
            rb.velocity = Vector3.zero;
        }
    }
    public void TakeDamage(int damage, Vector3 playerPos)
    {
        currentHealth -= damage;

        // �˹�
        Vector2 damageDir = Vector3.Normalize(transform.position - playerPos) * 40f;
        rb.velocity = Vector2.zero;
        rb.AddForce(damageDir * 50);

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
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            TakeDamage(collision.gameObject.GetComponent<PlayerController>().damage, collision.gameObject.transform.position);
        }
    }
}
