using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DefaultColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 850f; } }

    //Temporal Setting : Default Color Attack -> Sword
    public void Attack(PlayerController player)
    {
        if (player.doAttack && player.canAttack)
        {
            Debug.Log("Attak");
            player.canAttack = false;
            player.animator.SetBool("IsAttacking", true);
            player.UpdateCanAttack();
            //StartCoroutine(AttackCooldown(player));
            //attack
        }
    }


    IEnumerator AttackCooldown(PlayerController player)
    {
        yield return new WaitForSeconds(0.25f);
        player.canAttack = true;
    }
    //private void AttackCooldown(PlayerController player)
    //{
    //    player.canAttack = true;
    //}
}
