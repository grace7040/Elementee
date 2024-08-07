using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 30f; // 점프 힘 조절용 변수
    private Animator anim;
    Vector3 jumpVector;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        jumpVector = transform.up.normalized * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 발판의 충돌을 확인합니다.
        //if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log(jumpVector);
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // 플레이어 객체에 점프 힘을 적용합니다.
            if (playerRigidbody != null)
            {
                //Vector2 upDirection = transform.TransformDirection(Vector2.up);
                //upDirection = upDirection.normalized;
                //collision.rigidbody.AddForce(upDirection * jumpVector.magnitude, ForceMode2D.Impulse);
                playerRigidbody.velocity = jumpVector;// Vector2.zero;
                collision.rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            }

            // 애니메이션 재생
            anim.Play("spring", -1, 0.3f);
            // 효과음 재생
            AudioManager.Instance.PlaySFX("Spring");
        }
    }
}
