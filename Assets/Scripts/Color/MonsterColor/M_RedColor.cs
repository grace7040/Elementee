using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RedColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 50; } }

    public void Attack(MonsterController monster)
    {
        monster.GetComponent<Animator>().SetBool("IsAttacking", true);
        monster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject Fire = Instantiate(Resources.Load("Fire"), monster.transform.position, Quaternion.identity) as GameObject;
        Destroy(Fire, 2f);
    }
}