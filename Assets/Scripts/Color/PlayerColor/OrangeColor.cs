using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 2f; } }

    //모래 폭풍
    public void Attack(PlayerController player)
    {
        player.canAttack = false;
        //player.animator.SetBool("IsOrangeAttacking", true);
        player.gameObject.layer = 10; // layer 변경으로 충돌 처리 막음
        AudioManager.Instacne.PlaySFX("Orange");
        //4초 이후에 off
        player.CallOnDelay(CoolTime, () =>
        {
            //player.animator.SetBool("IsOrangeAttacking", false);
            player.GetComponent<PlayerController>().orange_WeaponEffect.SetActive(false);
            player.canAttack = true;
            player.gameObject.layer = 3;

        }
        );

    }
}
