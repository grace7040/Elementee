using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //�÷��̾�� ����� �浹�ϸ� ������ ���� �������� �˴ϴ�.
            GameManager.Instance.savePoint = transform;
            print("���̺�����");
        }
    }
}
