using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Rotate : MonoBehaviour
{
    public Transform centerPoint; // ȸ�� �߽���(Transform) ������ ����
    public float rotationSpeed = 30.0f; // ȸ�� �ӵ� ������ ����

    void Update()
    {
        if (centerPoint == null)
        {
            Debug.LogError("Center point not assigned! Please assign a center point in the Inspector.");
            return;
        }

        // ȸ�� �߽���(centerPoint)�� �������� ��ü�� ȸ����ŵ�ϴ�.
        transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
