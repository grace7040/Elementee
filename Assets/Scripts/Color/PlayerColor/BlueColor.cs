using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 15; } }
    public bool WallSliding { get { return false; } }


    //Temporal Setting : BLue Color Attack -> Throw Water obj
    public void Attack(PlayerController player)
    {

        player.canAttack = false;
        player.animator.SetBool("IsBlueAttacking", true);
        AudioManager.Instacne.PlaySFX("Blue");

        var throwableWeapon = ObjectPoolManager.Instance.GetGo("BlueWeapon");
        throwableWeapon.GetComponent<ThrowableWeapon>().Throw(player.transform.position, player.transform.localScale.x);


        player.CallOnDelay(1f, () =>
        {
            player.canAttack = true;
        });
        

    }
}
