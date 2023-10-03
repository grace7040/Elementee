using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public float moveSpeed = 1f;   // ����ٴϴ� �ӵ�

    void Update()
    {
        // ��� ������Ʈ�� ���� �̵�
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = playerPos.transform.position - transform.position;
        direction.Normalize(); // ���� ���͸� ����ȭ�Ͽ� �ӵ��� ����
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
