using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleColor : IColorState
{
    public int Damage { get { return 50; } }
    public bool WallSliding { get { return true; } }
    public float CoolTime { get { return 0.7f; } }

    public Action<string, bool> SetPlayerAnimatorBool = null;
    public Action ShakeCamera = null;
    public PurpleColor(Action<string, bool> setPlayerAnimatorBoolAction, Action shakeCamera)
    {
        ColorManager.Instance.HasRed = false;
        ColorManager.Instance.HasBlue = false;
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
        ShakeCamera = shakeCamera;
    }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        ShakeCamera();
        SetPlayerAnimatorBool("IsPurpleAttacking", true);
        AudioManager.Instance.PlaySFX("Purple");
    }
}
