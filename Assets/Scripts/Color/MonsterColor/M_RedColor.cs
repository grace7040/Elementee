using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M_RedColor : M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 15; } }

    public int M_health { get { return 100; } }

    public void Attack(MonsterController monster)
    {
        var fire = ObjectPoolManager.Instance.GetGo("Fire");

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