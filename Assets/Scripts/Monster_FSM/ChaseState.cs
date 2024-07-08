using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(MonController monster) : base(monster) { }

    public override void Enter()
    {
        monster.animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        if (Vector2.Distance(monster.transform.position, monster.player.position) <= monster.attackRange)
        {
            monster.ChangeState(new AttackState(monster));
        }
        else
        {
            if (monster.CheckGround())
            {
                Vector2 moveDirection = (monster.player.position - monster.transform.position);
                moveDirection.y = 0;
                moveDirection.Normalize();
                monster.rb.velocity = moveDirection * monster.moveSpeed;
            }
            else
            {
                monster.rb.velocity += Time.deltaTime * Vector2.down;
            }
        }
    }

    public override void Exit()
    {
        monster.animator.SetBool("IsWalking", false);
    }
}