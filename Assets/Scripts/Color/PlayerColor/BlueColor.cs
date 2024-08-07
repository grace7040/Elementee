using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColor : IColorState
{
    public int Damage { get { return 35; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    public Action<string, bool> SetPlayerAnimatorBool = null;
    public BlueColor(Action<string, bool> setPlayerAnimatorBoolAction)
    {
        ColorManager.Instance.HasBlue = false;
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
    }
    //Temporal Setting : BLue Color Attack -> Throw Water obj
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {

        //player.canAttack = false;
        SetPlayerAnimatorBool("IsBlueAttacking", true);
        AudioManager.Instance.PlaySFX("Blue");

        var throwableWeapon = ObjectPoolManager.Instance.GetGameObject("BlueWeapon");
        throwableWeapon.GetComponent<ThrowableWeapon>().Throw(playerPosition, playerLocalScaleX);        

    }
}
