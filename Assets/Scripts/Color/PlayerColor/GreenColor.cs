using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenColor : IColorState
{
    public int Damage { get { return 30; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    public Action<string, bool> SetPlayerAnimatorBool = null;
    public GreenColor(Action<string, bool> setPlayerAnimatorBoolAction)
    {
        ColorManager.Instance.HasYellow = false;
        ColorManager.Instance.HasBlue = false;
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
    }
    //Temporal Setting : Green Color Attack -> Throw leaf obj
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {

        //player.canAttack = false;
        SetPlayerAnimatorBool("IsGreenAttacking", true);
        AudioManager.Instacne.PlaySFX("Green");

        var throwableWeapon = ObjectPoolManager.Instance.GetGo("GreenWeapon");
        throwableWeapon.GetComponent<ThrowableWeapon>().Throw(playerPosition, playerLocalScaleX);
        
    }
}
