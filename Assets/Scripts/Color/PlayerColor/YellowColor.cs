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
    //Temporal Setting : Yellow Color Attack -> ���� ����
    public void Attack(PlayerController player)
    {
        //���� 
        //playerprefab on
        //player.canAttack = false;
        player.yellow_WeaponEffect.SetActive(true);
        player.yellow_WeaponEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.yellow].sprite;
        AudioManager.Instacne.PlaySFX("Yellow");
        //3�� ���Ŀ� off
        // ��� ������ ó������ ���� �־�� �ϴ� ��Ÿ�� ���� �κ� �ּ�
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
