using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor :  IColorState
{
    public int Damage { get { return 50; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.35f; } }

    public Action<string, bool> SetPlayerAnimatorBool = null;
    public RedColor(Action<string, bool> setPlayerAnimatorBoolAction)
    {
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
    }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        SetPlayerAnimatorBool("IsRedAttacking", true);
        AudioManager.Instacne.PlaySFX("Red");
    }
}
