using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int flagNum = 0;

    //private void Start()
    //{
    //    // �÷��̾��� ���� ������ ���
    //    if (flagNum == GameManager.Instance.mapFlag[GameManager.Instance.currentMapNum])
    //        GameManager.Instance.sponPos = this.transform;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instacne.PlaySFX("CheckPoint");
            //�÷��̾�� ����� �浹�ϸ� ����
            //GameManager.Instance.mapFlag[GameManager.Instance.currentMapNum] = flagNum;
            GameManager.Instance.RevivalPos = this.transform;
            //DataManager.Instance.JsonSave();
        }
    }
}
