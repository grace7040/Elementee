using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public float moveSpeed = 1f;   // ����ٴϴ� �ӵ�

    private void Start()
    {
        
    }
    void Update()
    {
        // ��� ������Ʈ�� ���� �̵�
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = new Vector3(playerPos.transform.position.x - transform.position.x, 0, 0);
        direction.Normalize(); // ���� ���͸� ����ȭ�Ͽ� �ӵ��� ����
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
