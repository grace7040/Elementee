using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RedColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 15; } }

    public void Attack(MonsterController monster)
    {
        #region old
        //monster.GetComponent<Animator>().SetBool("IsAttacking", true);

        ////monster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ////monster.CallOnDelay(1f, () => { GameObject Fire = Instantiate(Resources.Load("Fire"), monster.transform.position, Quaternion.identity) as GameObject; });

        //Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        //if (player != null)
        //{
        //    float distance = player.position.x - monster.transform.position.x;

        //    if (distance < 0f)
        //    {
        //        GameObject Fire = Instantiate(Resources.Load("Monster/Fire"), monster.transform.position - monster.transform.right.normalized * 0.0f, Quaternion.Euler(0, 0, -90)) as GameObject;
        //    }
        //    else if (distance > 0f)
        //    {
        //        GameObject Fire = Instantiate(Resources.Load("Monster/Fire"), monster.transform.position + monster.transform.right.normalized * 0.0f, Quaternion.Euler(0, 0, 90)) as GameObject;
        //    }
        //}
        ////GameObject Fire = Instantiate(Resources.Load("Fire"), monster.transform.position + monster.transform.right.normalized * 2.0f, Quaternion.identity) as GameObject;
        #endregion


    }
}