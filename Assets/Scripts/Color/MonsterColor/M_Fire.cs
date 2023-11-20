using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            // 몬스터와 플레이어의 위치 차이를 계산합니다.
            float distance = player.position.x - transform.position.x;

            // 플레이어가 몬스터의 왼쪽에 있으면 좌우를 뒤집습니다.
            if (distance < 0f)
            {
                sprite.flipX = true;
            }
            // 플레이어가 몬스터의 오른쪽에 있으면 좌우를 뒤집지 않습니다.
            else if (distance > 0f)
            {
                sprite.flipX = false;
            }
        }

        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
