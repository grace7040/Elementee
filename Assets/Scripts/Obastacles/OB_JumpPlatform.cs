using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 30f; // ���� �� ������ ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // �÷��̾� ��ü�� ���� ���� �����մϴ�.
            if (playerRigidbody != null)
            {
                Vector3 jumpVector = Vector3.up * jumpForce;
                playerRigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            }
        }
    }
}
