using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultColor : ColorState
{
    public override int JumpForce { get { return 1; } }
    public override void Attack()
    {
        Debug.Log("Default");
    }
}
