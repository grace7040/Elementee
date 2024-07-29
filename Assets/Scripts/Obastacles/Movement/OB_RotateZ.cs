using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_RotateZ : MonoBehaviour
{
    public Transform centerPoint; // ȸ�� �߽���(Transform) ������ ����
    public bool rightDir = true;
    public float rotationSpeed = 30.0f; // ȸ�� �ӵ� ������ ����

    Vector3 dir;

    private void Start()
    {
        if (rightDir)
            dir = Vector3.back;
        else
            dir = Vector3.forward;
    }

    void Update()
    {
        if (centerPoint == null)
        {
            Debug.Log("Center point not assigned! Please assign a center point in the Inspector.");
            return;
        }
        

        // ȸ�� �߽���(centerPoint)�� �������� ��ü�� ȸ����ŵ�ϴ�.
        transform.RotateAround(centerPoint.position, dir, rotationSpeed * Time.deltaTime);
    }
}
