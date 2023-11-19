using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlood : PoolAble
{
    private void Start()
    {
        this.CallOnDelay(0.5f, ReleaseObject);
    }
}
