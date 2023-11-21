using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pivotPoint; // 발판의 중점
    public float moveRange = 5.0f; // 발판의 상하 운동 범위
    public float moveSpeed = 2.0f; // 이동 속도

    private Vector3 originalPosition; // 발판의 초기 위치

    void Start()
    {
        originalPosition = transform.position; // 발판의 초기 위치를 기록
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveRange; // Sin 함수를 사용하여 상하 운동 계산
        transform.position = new Vector3(originalPosition.x, pivotPoint.position.y + newY, originalPosition.z);
    }

}
