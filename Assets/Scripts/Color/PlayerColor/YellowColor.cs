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

    //Temporal Setting : Yellow Color Attack -> ���� ����
    public void Attack(PlayerController player)
    {
        //���� 
        //playerprefab on
        player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(true);
        //Ư�� �ð� ���Ŀ� off
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
