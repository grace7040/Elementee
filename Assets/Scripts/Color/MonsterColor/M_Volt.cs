using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public GameObject Object; // 따라다닐 대상 오브젝트
    public float moveSpeed = 1f;   // 따라다니는 속도

    void Update()
    {
        if (Object != null)
        {
            // 대상 오브젝트를 향해 이동
            Vector3 direction = Object.transform.position - transform.position;
            direction.Normalize(); // 방향 벡터를 정규화하여 속도에 곱함
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
