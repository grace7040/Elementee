using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAbleM_weapons : PoolAble
{
    private void OnEnable()
    {
        this.CallOnDelay(1.0f, ReleaseObject);
    }
}