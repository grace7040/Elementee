using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DefaultColor : M_ColorState
{
    public override float JumpForce { get { return 850f; } }
    public override void Attack()
    {
        Debug.Log("Default");
    }
}
