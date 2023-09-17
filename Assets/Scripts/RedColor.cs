using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor : ColorState
{
    public override float JumpForce { get { return 800f; } }
    public override void Attack()
    {
        Debug.Log("Red");
    }
}
