using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int flagNum = 0;

    //private void Start()
    //{
    //    // 플레이어의 스폰 지점일 경우
    //    if (flagNum == GameManager.Instance.mapFlag[GameManager.Instance.currentMapNum])
    //        GameManager.Instance.sponPos = this.transform;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instacne.PlaySFX("CheckPoint");
            //플레이어와 깃발이 충돌하면 저장
            //GameManager.Instance.mapFlag[GameManager.Instance.currentMapNum] = flagNum;
            GameManager.Instance.RevivalPos = this.transform;
            //DataManager.Instance.JsonSave();
        }
    }
}
