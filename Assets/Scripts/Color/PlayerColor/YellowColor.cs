using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }

    //void Start()
    //{
    //    ColorManager.Instance.OnSaveColor += SetCustomSprite;
    //}
    //Temporal Setting : Yellow Color Attack -> 근접 공격
    public void Attack(PlayerController player)
    {
        //전기 
        //playerprefab on
        player.canAttack = false;
        player.yellowAttackEffect.SetActive(true);
        player.yellowAttackEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.yellow].sprite;
        AudioManager.Instacne.PlaySFX("Yellow");
        //3초 이후에 off
        player.CallOnDelay(0.5f, () =>
        {
            player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(false);
            player.canAttack = true;
        }
        ); 
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
