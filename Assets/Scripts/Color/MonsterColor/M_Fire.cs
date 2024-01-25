using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        //Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        //GameObject Red = GameObject.Find("M_Red");

        Destroy(gameObject, 2.0f);

        //if (player != null)
        //{
        //    // 몬스터와 플레이어의 위치 차이를 계산합니다.
        //    float distance = player.position.x - Red.gameObject.transform.position.x;

        //    // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
        //    if (distance < 0f)
        //    {
        //        sprite.flipX = true;
        //    }
        //    // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
        //    else if (distance > 0f)
        //    {
        //        sprite.flipX = false;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
}
