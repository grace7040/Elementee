using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DefaultColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 850f; } }
    public int Damage { get { return 10; } }


    public GameObject throwableObject { get; set; }
    public Sprite sprite { get; set; }

    //Temporal Setting : Default Color Attack -> Sword
    public void Attack(PlayerController player)
    {
        if (player.doAttack && player.canAttack)
        {
            Debug.Log("Attak");
            player.canAttack = false;
            player.animator.SetBool("IsAttacking", true);
            player.UpdateCanAttack();

        }
    }


}
