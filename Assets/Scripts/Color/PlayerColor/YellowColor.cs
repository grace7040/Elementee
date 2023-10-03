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
        //특정 시간 이후에 off
        //StartCoroutine(AttackCooldown(player.GetComponent<PlayerController>().yellowAttackEffect));
        player.CallDelay(3f, () =>
        {

        }
        );


    }


    IEnumerator AttackCooldown(GameObject effect)
    {
        yield return new WaitForSeconds(5f);
        effect.SetActive(false);
    }



}
