using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class M_General : MonoBehaviour
{
    public float moveSpeed = 2f;         // ���� �̵� �ӵ�
    public float changeDestinationTime = 5f; // ������ ���� �ֱ� (��)

    public int maxHealth = 100;
    private int currentHealth;

    private Vector3 randomDestination;    // ���� ������
    private NavMeshAgent navMeshAgent;    // NavMeshAgent ������Ʈ

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
            // ������ ������ ����
            randomDestination = GetRandomDestination();

            // ������ ����
            navMeshAgent.SetDestination(randomDestination);

            // ���� ������ ���� ���
            yield return new WaitForSeconds(changeDestinationTime);
        }
    }

    Vector3 GetRandomDestination()
    {
        // ������ ��ġ�� �����Ͽ� ��ȯ
        Vector3 randomDirection = Random.insideUnitSphere * 10f; // �ݰ� 10�� ���� ��ġ ����
        randomDirection += transform.position; // ���� ��ġ�� �߽����� �̵�

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
            TakeDamage(other.GetComponentInParent<PlayerController>().damage); // �̰� �³�?
        }
        else if (other.tag == "WeaponB")
        {
            TakeDamage(20);
            Destroy(other.gameObject, 0.1f);
        }
    }
}
