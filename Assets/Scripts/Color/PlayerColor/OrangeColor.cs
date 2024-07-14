using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrangeColor : IColorState
{
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 6f; } }

    public float durationTime = 5f;
    //모래 폭풍
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        AudioManager.Instacne.PlaySFX("Orange");
        // :: TODO :: 리팩토링을 위해 잠시 주석하겠습니다. 나중에 수정해야함.

        //player.gameObject.layer = 10; // layer 변경으로 충돌 처리 막음

        //// 그린 무기 + effect 켜기
        //player.OrangeWeaponEffect.SetActive(true);

        ////이동속도 up
        //player.m_MoveSpeed = 20f;


        //// 지속시간
        //player.CallOnDelay(durationTime, () =>
        //{
        //    player.OrangeWeaponEffect.SetActive(false);
        //    player.gameObject.layer = 3;
        //    player.m_MoveSpeed = 10f;

        //    //이동수단 down
        //});


        // :: END ::

    }


}