using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public float moveSpeed = 1.5f;   // ����ٴϴ� �ӵ�
    //public GameObject parentObject;

    void Start()
    {
        Destroy(gameObject, 2.0f);

        // ���� ��ü(child)�� ���� ��ü(parent)�� ������ ����ϴ�.
        //gameObject.transform.SetParent(parentObject.transform);
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
