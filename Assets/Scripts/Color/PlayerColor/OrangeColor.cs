using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 40; } }
    public bool WallSliding { get { return false; } }

    //�� ��ǳ
    public void Attack(PlayerController player)
    {
        player.canAttack = false;
        player.animator.SetBool("IsOrangeAttacking", true);
        AudioManager.Instacne.PlaySFX("Orange");
        //4�� ���Ŀ� off
        player.CallOnDelay(2f, () =>
        {
            player.animator.SetBool("IsOrangeAttacking", false);
            player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(false);
            player.canAttack = true;
        }
        );

    }
}
