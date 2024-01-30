using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 35; } }
    public bool WallSliding { get { return true; } }
    public float CoolTime { get { return 0.7f; } }


    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        player.GetComponent<PlayerController>().cam.GetComponent<FollowCamera>().ShakeCamera();

        //Debug.Log("Attak");
        player.canAttack = false;
        player.animator.SetBool("IsPurpleAttacking", true);
        AudioManager.Instacne.PlaySFX("Purple");
        player.CallOnDelay(CoolTime, () =>
        {
            player.canAttack = true;
        });
        
        
    }
}
