using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M_RedColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 15; } }

    public int M_health { get { return 100; } }

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
        // 딜레이가 조금 필요할듯?
        GameObject fire = ObjectPoolManager.Instance.GetGo("Fire");

        // 방향 처리
        SpriteRenderer monsterSpriteRenderer = monster.GetComponent<SpriteRenderer>();
        if (monsterSpriteRenderer.flipX)
        {
            fire.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            fire.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }

        Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
        fire.transform.SetParent(rb.transform);
        fire.transform.localPosition = Vector3.zero;
    }
}