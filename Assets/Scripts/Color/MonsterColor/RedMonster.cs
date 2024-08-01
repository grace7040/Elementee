using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMonster : MonsterController
{
    public GameObject FireObject;

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

    public override void Attack()
    {
        FireObject.SetActive(true);
        this.CallOnDelay(1.5f, () => FireObject.SetActive(false));
    }
}