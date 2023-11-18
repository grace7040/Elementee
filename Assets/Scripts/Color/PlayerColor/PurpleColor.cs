using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return true; } }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        player.GetComponent<PlayerController>().cam.GetComponent<FollowCamera>().ShakeCamera();
        //if (player.canAttack)
        {
            Debug.Log("Attak");
            player.canAttack = false;
            player.animator.SetBool("IsPurpleAttacking", true);
            player.CallOnDelay(3f, () =>
            {
                player.canAttack = true;
            });
        }
        
    }
}
