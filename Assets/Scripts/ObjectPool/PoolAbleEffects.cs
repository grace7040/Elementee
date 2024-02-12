using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAbleEffects : PoolAble
{
    private void OnEnable()
    {
        this.CallOnDelay(1.5f, ReleaseObject);
    }
}
