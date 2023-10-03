using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_YellowColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 10; } }

    public void Attack(MonsterController monster)
    {
        //monster.animator.SetBool("IsAttacking", true);
        GameObject Volt = Instantiate(Resources.Load("Volt"), monster.transform.position, Quaternion.identity) as GameObject;
        Destroy(Volt, 2f);
        Debug.Log("Yellow");
    }
}
