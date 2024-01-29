using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Rotate : MonoBehaviour
{
    public Transform centerPoint; // 회전 중심점(Transform) 지정용 변수
    public float rotationSpeed = 30.0f; // 회전 속도 조절용 변수

    void Update()
    {
        if (centerPoint == null)
        {
            Debug.LogError("Center point not assigned! Please assign a center point in the Inspector.");
            return;
        }

        // 회전 중심점(centerPoint)을 기준으로 객체를 회전시킵니다.
        transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
