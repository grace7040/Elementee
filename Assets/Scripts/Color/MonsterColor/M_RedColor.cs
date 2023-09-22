using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RedColor : M_ColorState
{
    public override float JumpForce { get { return 800f; } }
    public override void Attack()
    {
        Debug.Log("Red");
    }
}
