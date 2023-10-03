using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }

    public GameObject ThrowableObject { get; set; }

    public GameObject CustomObject { get; set; }

    public Sprite Sprite { get; set; }

    //Temporal Setting : Yellow Color Attack -> 근접 공격
    public void Attack(PlayerController player)
    {
        //전기 
        //playerprefab on
        player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(true);

        //3초 이후에 off
        player.CallDelay(3f, () =>
        {
            player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(false);
        }
        );


    }

}
