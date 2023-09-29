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
            m_JumpForce = value.JumpForce;
        }
    }

    public int maxHealth = 100;
    private int currentHealth;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody rb;

    public GameObject itemPrefab; // 떨어뜨릴 아이템 프리팹

    private float m_JumpForce;

    public bool canAttack = true;

    public Animator animator;

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
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; // "Player" 태그를 가진 오브젝트를 플레이어로 설정
    }

    private void Update()
    {
        //if (플레이어에게 공격 받을 시)
        //{
        //    TakeDamage(공격력);
        //}

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 공격 범위 안에 있고 공격 쿨다운이 끝났으면 공격 실행
        if (distanceToPlayer <= attackRange && canAttack)
        {
            color.Attack(this);
            UpdateCanAttack();
        }
        // 플레이어가 감지 범위 안에 있으면 플레이어를 향해 이동
        else if (distanceToPlayer <= detectionRange)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;
        }
        else
        {
            // 감지 범위를 벗어난 경우 이동 중지
            rb.velocity = Vector3.zero;
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

    public void UpdateCanAttack()
    {
        StartCoroutine(AttackCooldown());
    }
}
