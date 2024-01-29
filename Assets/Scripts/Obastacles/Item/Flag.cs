using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            AudioManager.Instacne.PlaySFX("CheckPoint");
            //�÷��̾�� ����� �浹�ϸ� ������ ���� �������� �˴ϴ�.
            GameManager.Instance.savePoint[0] = transform.position.x;
            GameManager.Instance.savePoint[1] = transform.position.y;
            GameManager.Instance.savePoint[2] = transform.position.z;
        }
    }
}
