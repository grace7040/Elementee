using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VerticlaMovement : MonoBehaviour
{
    public Transform centerPoint; // 중심점 지정용 변수
    public float verticalSpeed = 1.0f; // 상하 움직임의 속도 조절용 변수
    public float verticalRange = 1.0f; // 상하 움직임의 범위 조절용 변수

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        // 상하 움직임 계산
        float verticalMovementValue = Mathf.Sin(Time.time * verticalSpeed) * verticalRange * 0.01f;

        // 새로운 위치 설정
        Vector3 offsetFromCenter = transform.position - centerPoint.position;
        offsetFromCenter.y = 0; // 중심점과의 높이 차이를 제거합니다.
        Vector3 newPosition = centerPoint.position + offsetFromCenter + Vector3.up * verticalMovementValue;

        // 객체의 위치 업데이트
        transform.position = newPosition;
    }
}
