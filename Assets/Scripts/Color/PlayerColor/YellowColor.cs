using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return false; } }

    //void Start()
    //{
    //    ColorManager.Instance.OnSaveColor += SetCustomSprite;
    //}
    //Temporal Setting : Yellow Color Attack -> ���� ����
    public void Attack(PlayerController player)
    {
        //���� 
        //playerprefab on
        player.canAttack = false;
        player.yellowAttackEffect.SetActive(true);
        player.yellowAttackEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.yellow].sprite;
        //3�� ���Ŀ� off
        player.CallOnDelay(3f, () =>
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
