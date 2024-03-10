using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_RotateZ : MonoBehaviour
{
    public Transform centerPoint; // 회전 중심점(Transform) 지정용 변수
    public bool rightDir = true;
    public float rotationSpeed = 30.0f; // 회전 속도 조절용 변수

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
        

        // 회전 중심점(centerPoint)을 기준으로 객체를 회전시킵니다.
        transform.RotateAround(centerPoint.position, dir, rotationSpeed * Time.deltaTime);
    }
}
