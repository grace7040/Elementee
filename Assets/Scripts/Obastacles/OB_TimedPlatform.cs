using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_TimedPlatform : MonoBehaviour
{
    public float delayTime = 2f; // ���������� ��� �ð�
    private bool playerOnPlatform = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            this.CallOnDelay(delayTime, DisappearPlatform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾�� ���� ������ �浹�� �������� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    private void DisappearPlatform()
    {
        // �÷��̾ ���� ���� ���� ���� ������ �����մϴ�.
        if (playerOnPlatform)
        {
            Destroy(gameObject);
        }
    }
}
