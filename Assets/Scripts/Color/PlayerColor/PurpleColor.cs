using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 35; } }
    public bool WallSliding { get { return true; } }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        player.GetComponent<PlayerController>().cam.GetComponent<FollowCamera>().ShakeCamera();

        //Debug.Log("Attak");
        player.canAttack = false;
        player.animator.SetBool("IsPurpleAttacking", true);
        AudioManager.Instacne.PlaySFX("Purple");
        player.CallOnDelay(0.3f, () =>
        {
            player.canAttack = true;
        });
        
        
    }
}
