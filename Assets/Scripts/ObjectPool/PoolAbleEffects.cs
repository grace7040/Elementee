using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAbleEffects : PoolAble
{
    public float destroyTime = 1.5f;
    private void OnEnable()
    {
        this.CallOnDelay(destroyTime, ReleaseObject);
    }
}
