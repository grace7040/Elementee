using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_TimedPlatform : MonoBehaviour
{
    public float delayTime = 2f; // ���������� ��� �ð�
    private bool playerOnPlatform = false;

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� ������ �浹�� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            Invoke("DisappearPlatform", delayTime);
        }
    }

    private void OnCollisionExit(Collision collision)
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
