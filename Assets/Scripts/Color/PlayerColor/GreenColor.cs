using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenColor : IColorState
{
    public int Damage { get { return 30; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }


    //Temporal Setting : Green Color Attack -> Throw leaf obj
    public void Attack(PlayerController player)
    {

        player.canAttack = false;
        player.animator.SetBool("IsGreenAttacking", true);
        AudioManager.Instacne.PlaySFX("Green");

        var throwableWeapon = ObjectPoolManager.Instance.GetGo("GreenWeapon");
        throwableWeapon.GetComponent<ThrowableWeapon>().Throw(player.transform.position, player.transform.localScale.x);

        player.CallOnDelay(CoolTime, () =>
        {
            player.canAttack = true;
        });
        
    }
}
