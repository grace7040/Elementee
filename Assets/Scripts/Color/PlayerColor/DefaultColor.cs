using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefaultColor : IColorState
{
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.35f; } }

    public Action<string, bool> SetPlayerAnimatorBool = null;
    public DefaultColor(Action<string, bool> setPlayerAnimatorBoolAction)
    {
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
    }
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        SetPlayerAnimatorBool("IsAttacking", true);
    }
}
