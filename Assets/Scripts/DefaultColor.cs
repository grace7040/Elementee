using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultColor : ColorState
{
    public override float JumpForce { get { return 400f; } }
    public override void Attack()
    {
        Debug.Log("Default");
    }
}
