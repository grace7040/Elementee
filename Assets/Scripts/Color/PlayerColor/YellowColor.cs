using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : IColorState
{
    public int Damage { get { return 15; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 3f; } }


    //void Start()
    //{
    //    ColorManager.Instance.OnSaveColor += SetCustomSprite;
    //}
    //Temporal Setting : Yellow Color Attack -> 근접 공격
    public void Attack(Vector3 playerPosition, float playerLocalScaleX)
    {
        //전기 
        // :: TODO :: 리팩토링을 위해 잠시 주석하겠습니다. 나중에 수정해야함.
        //player.YellowWeaponEffect.SetActive(true);
        //player.YellowWeaponEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.ColorWeapons[(int)Colors.Yellow].sprite;
        // :: END ::
        
        AudioManager.Instacne.PlaySFX("Yellow");

        //3초 이후에 off
        // 노란 공격은 처음부터 켜져 있어야 하니 쿨타임 적용 부분 주석
        //player.CallOnDelay(CoolTime, () =>
        //{
        //    player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(false);
        //    player.canAttack = true;
        //}
        //); 
    }

    //void SetCustomSprite()
    //{
    //    PlayerController player = FindObjectOfType<PlayerController>();

    //    // ::TEST::
    //    player.yellowAttackEffect.GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.yellow].sprite;
    //    // ::TEST::

    //    ColorManager.Instance.OnSaveColor -= SetCustomSprite;
    //}

}