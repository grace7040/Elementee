using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor :  IColorState
{
    public float JumpForce { get { return 850f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return false; } }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        //if (player.doAttack && player.canAttack)
        {
            Debug.Log("Attak");
            player.canAttack = false;
            player.animator.SetBool("IsRedAttacking", true);
            //player.UpdateCanAttack();
            player.CallOnDelay(0.3f, () =>
            {
                player.canAttack = true;
            });

        }
    }
}
