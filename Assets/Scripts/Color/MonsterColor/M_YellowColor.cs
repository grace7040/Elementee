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
        
        Debug.Log("Yellow");
        //StartCoroutine(Light());
    }
    IEnumerator Light()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
