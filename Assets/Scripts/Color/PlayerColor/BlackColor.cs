using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : IColorState
{
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    Action OnBlackAttacked = null;

    public BlackColor(Action onBlackAttackedAction)
    {
        ColorManager.Instance.HasColor(Colors.Red, false);
        ColorManager.Instance.HasColor(Colors.Yellow, false);
        ColorManager.Instance.HasColor(Colors.Blue, false);
        OnBlackAttacked = onBlackAttackedAction;
    }
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        OnBlackAttacked();
    }

    //public void SetPlayerColor(Colors mon_color)
    //{
    //    switch (mon_color)
    //    {
    //        case Colors.Default:
    //            break;
    //        case Colors.Red:
    //            ColorManager.Instance.HasRed = true;
    //            break;
    //        case Colors.Blue:
    //            ColorManager.Instance.HasBlue = true;
    //            break;
    //        case Colors.Yellow:
    //            ColorManager.Instance.HasYellow = true;
    //            break;
    //    }
    //}
}
