using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor :  IColorState
{
    public float JumpForce { get { return 850f; } }
    public int Damage { get { return 20; } }
    public bool WallSliding { get { return false; } }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        Debug.Log("Attak");
        player.canAttack = false;
        player.animator.SetBool("IsRedAttacking", true);
        AudioManager.Instacne.PlaySFX("Red");
        //player.UpdateCanAttack();
        player.CallOnDelay(0.3f, () =>
            {
                player.canAttack = true;
            });
    }
}
