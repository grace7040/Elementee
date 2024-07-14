using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : IColorState
{
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        // :: TODO :: 리팩토링을 위해 잠시 주석하겠습니다. 나중에 수정해야함.

        //if (player.IsHoldingEnemy)
        //{
        //    player.BlackThrow();
        //    AudioManager.Instacne.PlaySFX("BlackRelease");
        //}
        //else
        //{
        //    player.BlackPull();
        //    AudioManager.Instacne.PlaySFX("Black");
        //}

        // :: END ::
    }

    public void SetPlayerColor(Colors mon_color)
    {
        switch (mon_color)
        {
            case Colors.Default:
                break;
            case Colors.Red:
                ColorManager.Instance.HasRed = true;
                break;
            case Colors.Blue:
                ColorManager.Instance.HasBlue = true;
                break;
            case Colors.Yellow:
                ColorManager.Instance.HasYellow = true;
                break;
        }
    }
}
