using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //플레이어와 깃발이 충돌하면 마지막 저장 지점으로 됩니다.
            GameManager.Instance.savePoint = transform;
            print("세이브저장");
        }
    }
}
