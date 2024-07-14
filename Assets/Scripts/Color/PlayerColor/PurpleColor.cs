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
    public PurpleColor(Action<string, bool> setPlayerAnimatorBoolAction)
    {
        SetPlayerAnimatorBool = setPlayerAnimatorBoolAction;
    }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        // :: TODO :: 리팩토링을 위해 잠시 주석하겠습니다. 나중에 수정해야함.
        //player.GetComponent<PlayerController>().cam.GetComponent<FollowCamera>().ShakeCamera();
        // :: END ::

        SetPlayerAnimatorBool("IsPurpleAttacking", true);
        AudioManager.Instacne.PlaySFX("Purple");
        
    }
}
