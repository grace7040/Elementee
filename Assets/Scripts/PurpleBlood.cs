using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBlood : PoolAble
{
    private void Start()
    {
        this.CallOnDelay(0.5f, ReleaseObject);
    }
}
