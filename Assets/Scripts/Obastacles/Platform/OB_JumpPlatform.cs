using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 30f; // ���� �� ������ ����
    private Animator anim;
    Vector3 jumpVector;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        jumpVector = transform.up.normalized * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        //if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log(jumpVector);
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // �÷��̾� ��ü�� ���� ���� �����մϴ�.
            if (playerRigidbody != null)
            {
                //Vector2 upDirection = transform.TransformDirection(Vector2.up);
                //upDirection = upDirection.normalized;
                //collision.rigidbody.AddForce(upDirection * jumpVector.magnitude, ForceMode2D.Impulse);
                playerRigidbody.velocity = jumpVector;// Vector2.zero;
                collision.rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            }

            // �ִϸ��̼� ���
            anim.Play("spring", -1, 0.3f);
            // ȿ���� ���
            AudioManager.Instance.PlaySFX("Spring");
        }
    }
}
