using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Default : MonsterController
{
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isDie) return;
        Move();
    }
}
