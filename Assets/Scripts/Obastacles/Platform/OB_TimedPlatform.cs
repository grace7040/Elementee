using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_TimedPlatform : MonoBehaviour
{
    public float delayTime = 2f; // 사라지기까지 대기 시간
    private bool playerOnPlatform = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 발판의 충돌을 확인합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            this.CallOnDelay(delayTime, DisappearPlatform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어와 발판 사이의 충돌이 끝났음을 확인합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    private void DisappearPlatform()
    {
        // 플레이어가 발판 위에 있을 때만 발판을 제거합니다.
        if (playerOnPlatform)
        {
            Destroy(gameObject);
        }
    }
}
