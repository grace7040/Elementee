using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RedColor : MonoBehaviour, M_IColorState
{
    public float JumpForce { get { return 800f; } }
    public void Attack(MonsterController monster)
    {
        monster.animator.SetBool("IsAttacking", true);
        Debug.Log("Red");
    }
}