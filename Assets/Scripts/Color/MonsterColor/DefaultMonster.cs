using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonster : MonsterController
{
    protected override void Start()
    {
        base.Start();
        stateMachine.ChangeState(new IdleState(this));
    }

    protected override void Update()
    {
        if (IsDie) return;
        base.Update();
    }
}