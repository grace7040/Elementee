using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f; // ���� �� ������ ����
    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // �÷��̾� ��ü�� ���� ���� �����մϴ�.
            if (playerRigidbody != null)
            {
                Vector3 jumpVector = Vector3.up * jumpForce;
                playerRigidbody.AddForce(jumpVector, ForceMode.Impulse);
            }
        }
    }
}
