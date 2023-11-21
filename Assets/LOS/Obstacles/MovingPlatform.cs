using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pivotPoint; // ������ ����
    public float moveRange = 5.0f; // ������ ���� � ����
    public float moveSpeed = 2.0f; // �̵� �ӵ�

    private Vector3 originalPosition; // ������ �ʱ� ��ġ

    void Start()
    {
        originalPosition = transform.position; // ������ �ʱ� ��ġ�� ���
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveRange; // Sin �Լ��� ����Ͽ� ���� � ���
        transform.position = new Vector3(originalPosition.x, pivotPoint.position.y + newY, originalPosition.z);
    }

}
