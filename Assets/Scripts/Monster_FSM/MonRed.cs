using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonRed : MonController
{
    public GameObject fireObject;

    protected override void Start()
    {
        base.Start();
        stateMachine.ChangeState(new IdleState(this));
    }

    protected override void Update()
    {
        if (isDie) return;
        base.Update();
    }

    public override void Attack()
    {
        fireObject.SetActive(true);
        this.CallOnDelay(1.5f, () => fireObject.SetActive(false));
    }
}
