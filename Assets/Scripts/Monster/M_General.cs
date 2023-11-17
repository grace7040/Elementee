using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class M_General : MonoBehaviour
{
    public float moveSpeed = 2f;         // 몬스터 이동 속도
    public float changeDestinationTime = 5f; // 목적지 변경 주기 (초)

    public int maxHealth = 100;
    private int currentHealth;

    private Vector3 randomDestination;    // 랜덤 목적지
    private NavMeshAgent navMeshAgent;    // NavMeshAgent 컴포넌트

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(ChangeDestination());
        currentHealth = maxHealth;
    }
    private void Update()
    {
        
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
        Destroy(gameObject);
    }

    IEnumerator ChangeDestination()
    {
        while (true)
        {
            // 랜덤한 목적지 생성
            randomDestination = GetRandomDestination();

            // 목적지 설정
            navMeshAgent.SetDestination(randomDestination);

            // 다음 목적지 변경 대기
            yield return new WaitForSeconds(changeDestinationTime);
        }
    }

    Vector3 GetRandomDestination()
    {
        // 랜덤한 위치를 생성하여 반환
        Vector3 randomDirection = Random.insideUnitSphere * 10f; // 반경 10의 랜덤 위치 생성
        randomDirection += transform.position; // 현재 위치를 중심으로 이동

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);

        return hit.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Weapon")
        {
            Debug.Log(123);
            TakeDamage(other.GetComponentInParent<PlayerController>().damage); // 이거 맞나?
        }
        else if (other.tag == "WeaponB")
        {
            TakeDamage(20);
            Destroy(other.gameObject, 0.1f);
        }
    }
}
