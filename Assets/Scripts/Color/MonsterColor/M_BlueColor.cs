using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BlueColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 10; } }

    public void Attack(MonsterController monster)
    {
        monster.GetComponent<Animator>().SetBool("IsAttacking", true);
        GameObject Water = Instantiate(Resources.Load("Water"), monster.transform.position, Quaternion.identity) as GameObject;
        //Debug.Log("Blue");
    }
}
