using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMonster : MonsterController
{
    GameObject Water;

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
        Water = ObjectPoolManager.Instance.GetGo("MonsterWater");
        Water.transform.position = transform.position;
        Water.GetComponent<M_Water>().direction = IsFlip ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
    }
}