using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 2f; } }

    //�� ��ǳ
    public void Attack(PlayerController player)
    {
        player.canAttack = false;
        //player.animator.SetBool("IsOrangeAttacking", true);
        player.gameObject.layer = 10; // layer �������� �浹 ó�� ����
        AudioManager.Instacne.PlaySFX("Orange");
        //4�� ���Ŀ� off
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
