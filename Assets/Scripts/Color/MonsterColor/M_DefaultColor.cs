using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DefaultColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 850f; } }

    public int M_damage { get { return 10; } }

    public void Attack(MonsterController monster)
    {
        //monster.animator.SetBool("IsAttacking", true);
        Debug.Log("Default");
    }
}