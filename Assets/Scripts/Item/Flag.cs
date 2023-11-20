using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            //플레이어와 깃발이 충돌하면 마지막 저장 지점으로 됩니다.
        }
    }
}
