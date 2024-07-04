using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : IColorState
{
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    public void Attack(PlayerController player)
    {
        player.CallOnDelay(CoolTime, () => { player.canAttack = true; });

        if (player.isHoldingEnemy)
        {
            player.BlackThrow();
            //player.canAttack = false;
            AudioManager.Instacne.PlaySFX("BlackRelease");
        }
        else
        {
            player.BlackPull();
            AudioManager.Instacne.PlaySFX("Black");
        }
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
