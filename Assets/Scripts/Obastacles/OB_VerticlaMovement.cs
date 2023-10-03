using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VerticlaMovement : MonoBehaviour
{
    public Transform centerPoint; // �߽��� ������ ����
    public float verticalSpeed = 2.0f; // ���� �������� �ӵ� ������ ����
    public float verticalRange = 2.0f; // ���� �������� ���� ������ ����

    private Vector3 initialPosition;

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogError("Center point not assigned! Please assign a center point in the Inspector.");
            enabled = false; // Disable the script to prevent errors
        }

        initialPosition = transform.position; // �ʱ� ��ġ ����
    }

    void Update()
    {
        // ���� ������ ���
        float verticalMovementValue = Mathf.Sin(Time.time * verticalSpeed) * verticalRange;

        // ���ο� ��ġ ����
        Vector3 offsetFromCenter = transform.position - centerPoint.position;
        offsetFromCenter.y = 0; // �߽������� ���� ���̸� �����մϴ�.
        Vector3 newPosition = centerPoint.position + offsetFromCenter + Vector3.up * verticalMovementValue;

        // ��ü�� ��ġ ������Ʈ
        transform.position = newPosition;
    }
}
