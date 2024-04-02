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
    public void Attack(PlayerController player)
    {
        //player.canAttack = false;
        player.gameObject.layer = 10; // layer 변경으로 충돌 처리 막음
        AudioManager.Instacne.PlaySFX("Orange");

        // 그린 무기 + effect 켜기
        player.orange_WeaponEffect.SetActive(true);
        //player.orange_Weapon.SetActive(true);

        //이동속도 up
        player.m_MoveSpeed = 20f;
     

        // 지속시간
        player.CallOnDelay(durationTime, () =>
        {
            player.orange_WeaponEffect.SetActive(false);
            player.gameObject.layer = 3;
            player.m_MoveSpeed = 10f;

            //이동수단 down
        });


        //쿨타임 
        player.CallOnDelay(CoolTime, () =>
        {
            player.canAttack = true;
        });


        //player.orange_WeaponEffect.GetComponent<SpriteRenderer>().DOFlip();
        //player.animator.SetBool("IsOrangeAttacking", true);
    }


}
