using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public GameObject Object; // ����ٴ� ��� ������Ʈ
    public float moveSpeed = 1f;   // ����ٴϴ� �ӵ�

    void Update()
    {
        if (Object != null)
        {
            // ��� ������Ʈ�� ���� �̵�
            Vector3 direction = Object.transform.position - transform.position;
            direction.Normalize(); // ���� ���͸� ����ȭ�Ͽ� �ӵ��� ����
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
