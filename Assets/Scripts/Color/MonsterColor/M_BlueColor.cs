using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BlueColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 10; } }

    GameObject Waters;

    public void Attack(MonsterController monster)
    {
        monster.GetComponent<Animator>().SetBool("IsAttacking", true);
        monster.CallOnDelay(1f, () => { GameObject Water = Instantiate(Resources.Load("Waters"), monster.transform.position, Quaternion.identity) as GameObject; });
        monster.CallOnDelay(1f, () => { Waters = Instantiate(Resources.Load("Blue_Attack_Effect_"), monster.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; });
        Destroy(Waters);
        
        //Debug.Log("Blue");
    }
}
