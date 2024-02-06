using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BlueColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 10; } }

    public int M_health { get { return 100; } }

    public void Attack(MonsterController monster)
    {
        GameObject Water = Instantiate(Resources.Load("Monster/Waters"), monster.transform.position, Quaternion.identity) as GameObject;
        GameObject Waters = Instantiate(Resources.Load("Monster/Blue_Attack_Effect_"), monster.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }
}
