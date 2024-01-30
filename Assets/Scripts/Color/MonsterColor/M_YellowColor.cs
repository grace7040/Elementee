using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_YellowColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 20; } }

    public void Attack(MonsterController monster)
    {
        // monster.GetComponent<Animator>().SetBool("IsAttacking", true);
        // monster.moveSpeed = 5.0f;
        //monster.CallOnDelay(1f, () => { GameObject Volt = Instantiate(Resources.Load("Volt"), monster.transform.position, Quaternion.identity) as GameObject; });
        // GameObject Volt = Instantiate(Resources.Load("Monster/Volt"), monster.transform.position, Quaternion.identity) as GameObject;
    }
}
