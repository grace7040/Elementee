using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_YellowColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 15; } }

    public int M_health { get { return 100; } }

    public void Attack(MonsterController monster)
    {
        GameObject volt = ObjectPoolManager.Instance.GetGo("Volt");
        Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
        volt.transform.SetParent(rb.transform);
        volt.transform.localPosition = Vector3.zero;
    }
}
