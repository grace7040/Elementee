using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor : ColorState
{
    public override int JumpForce { get { return 3; } }
    public override void Attack()
    {
        Debug.Log("Red");
    }
}
