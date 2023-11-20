using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volt : MonoBehaviour
{
    public float moveSpeed = 1.5f;   // 따라다니는 속도
    //public GameObject parentObject;

    void Start()
    {
        Destroy(gameObject, 2.0f);

        // 하위 객체(child)를 상위 객체(parent)의 하위로 만듭니다.
        //gameObject.transform.SetParent(parentObject.transform);
    }
    void Update()
    {
        // 대상 오브젝트를 향해 이동
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = new Vector3(playerPos.transform.position.x - transform.position.x, 0, 0);
        direction.Normalize(); // 방향 벡터를 정규화하여 속도에 곱함
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
