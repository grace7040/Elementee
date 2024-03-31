using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Default : MonsterController
{
    new void Update()
    {
        base.Update();
        if (isDie) return;

        if (!CheckGround())
        {
            rb.velocity += Time.deltaTime * Vector2.down;
        }
        else
        {
            Move();
        }
    }
}
