using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(MonsterController monster) : base(monster) { }

    public override void Enter()
    {
        monster.animator.SetBool("IsWalking", false);
    }

    public override void Update()
    {
        if (Vector2.Distance(monster.transform.position, monster.player.position) <= monster.detectionRange && monster.distanceY <= 1.0f)
        {
            monster.ChangeState(new ChaseState(monster));
        }
        else
        {
            if (monster.CheckGround())
            {
                monster.Move();
            }
            else
            {
                monster.rb.velocity += Time.deltaTime * Vector2.down;
            }
        }
    }

    public override void Exit() { }
}