using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : IColorState
{
    public int Damage { get { return 15; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 3f; } }

    Action OnYellowAttacked = null;

    public YellowColor(Action onYellowAttackedAction)
    {
        OnYellowAttacked = onYellowAttackedAction;
    }

    //Temporal Setting : Yellow Color Attack -> 근접 공격
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        OnYellowAttacked();
        AudioManager.Instacne.PlaySFX("Yellow");
    }
}