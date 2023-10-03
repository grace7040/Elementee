using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f; // 점프 힘 조절용 변수
    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 발판의 충돌을 확인합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // 플레이어 객체에 점프 힘을 적용합니다.
            if (playerRigidbody != null)
            {
                Vector3 jumpVector = Vector3.up * jumpForce;
                playerRigidbody.AddForce(jumpVector, ForceMode.Impulse);
            }
        }
    }
}
