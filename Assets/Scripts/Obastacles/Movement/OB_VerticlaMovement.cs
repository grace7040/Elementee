using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VerticlaMovement : MonoBehaviour
{
    private Transform centerPoint; // �߽��� ������ ����
    public float verticalSpeed = 1.0f; // ���� �������� �ӵ� ������ ����
    public float verticalRange = 1.0f; // ���� �������� ���� ������ ����

    void Start()
    {
        //GameObject obj = GameObject.Find("PlayerPrefab (1)");

        //foreach (Transform child in obj.transform)
        //{
        //    if (child.name == "WeaponPosition")
        //    {
        //        centerPoint = child;
        //    }
        //}

        centerPoint = transform;

    }

    void Update()
    {
        // ���� ������ ���
        float verticalMovementValue = Mathf.Sin(Time.time * verticalSpeed) * verticalRange * 0.01f;

        // ���ο� ��ġ ����
        Vector3 offsetFromCenter = transform.position - centerPoint.position;
        offsetFromCenter.y = 0; // �߽������� ���� ���̸� �����մϴ�.
        Vector3 newPosition = centerPoint.position + offsetFromCenter + Vector3.up * verticalMovementValue;

        // ��ü�� ��ġ ������Ʈ
        transform.position = newPosition;
    }
}
