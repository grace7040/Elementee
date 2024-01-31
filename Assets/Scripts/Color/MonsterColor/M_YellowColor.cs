using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_YellowColor : MonoBehaviour, M_IColorState
{
    public float M_JumpForce { get { return 800f; } }

    public int M_damage { get { return 20; } }

    public int M_health { get { return 100; } }

    public void Attack(MonsterController monster)
    {
        #region old
        // monster.GetComponent<Animator>().SetBool("IsAttacking", true);
        // monster.moveSpeed = 5.0f;
        //monster.CallOnDelay(1f, () => { GameObject Volt = Instantiate(Resources.Load("Volt"), monster.transform.position, Quaternion.identity) as GameObject; });
        // GameObject Volt = Instantiate(Resources.Load("Monster/Volt"), monster.transform.position, Quaternion.identity) as GameObject;
        #endregion
        GameObject volt = ObjectPoolManager.Instance.GetGo("Volt");
        Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
        volt.transform.SetParent(rb.transform);
        volt.transform.localPosition = Vector3.zero;
    }
}
