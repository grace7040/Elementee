using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 30f; // ���� �� ������ ����
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.Play("spring",-1,0.3f);
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // �÷��̾� ��ü�� ���� ���� �����մϴ�.
            if (playerRigidbody != null)
            {
                Vector3 jumpVector = Vector3.up * jumpForce;
                playerRigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            }
            AudioManager.Instacne.PlaySFX("Spring");
        }
    }
}
