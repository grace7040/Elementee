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
    public void Attack(PlayerController player)
    {
        //전기 
        //playerprefab on
        //player.canAttack = false;
        player.yellow_WeaponEffect.SetActive(true);
        player.yellow_WeaponEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.Yellow].sprite;
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