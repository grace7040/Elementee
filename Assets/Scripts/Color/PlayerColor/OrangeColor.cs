using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 40; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 2f; } }

    //¸ð·¡ ÆøÇ³
    public void Attack(PlayerController player)
    {
        player.canAttack = false;
        player.animator.SetBool("IsOrangeAttacking", true);
        AudioManager.Instacne.PlaySFX("Orange");
        //4ÃÊ ÀÌÈÄ¿¡ off
        player.CallOnDelay(CoolTime, () =>
        {
            player.animator.SetBool("IsOrangeAttacking", false);
            player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(false);
            player.canAttack = true;
        }
        );

    }
}
