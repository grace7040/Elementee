using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(MonsterController monster) : base(monster) { }

    public override void Enter()
    {
        monster.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        monster.IsFlip = monster.Player.position.x < monster.transform.position.x;
        monster.MonsterBody.rotation = monster.IsFlip ? Quaternion.identity : monster.FlipQuaternion;

        if (Vector2.Distance(monster.transform.position, monster.Player.position) <= monster.AttackRange && monster.DistanceY <= 0.7f)
        {
            monster.ChangeState(new AttackState(monster));
        }
        else
        {
            if (monster.CheckGround())
            {
                if (!monster.CheckCliff())
                {
                    if (Vector2.Distance(monster.transform.position, monster.Player.position) > monster.DetectionRange)
                    {
                        monster.ChangeState(new IdleState(monster));
                    }
                    else
                    {
                        monster.MoveDirection = (monster.Player.position - monster.transform.position);
                        monster.MoveDirection.y = 0;
                        monster.MoveDirection.Normalize();
                        monster.Rb.velocity = monster.MoveDirection * monster.MoveSpeed;
                    }
                }
                else
                {
                    monster.Rb.velocity = Vector2.zero;

                    if (Vector2.Distance(monster.transform.position, monster.Player.position) > monster.DetectionRange)
                    {
                        monster.ChangeState(new IdleState(monster));
                    }
                }
            }
            else
            {
                monster.Rb.velocity += Time.deltaTime * Vector2.down;
            }
        }
    }

    public override void Exit()
    {
        monster.Animator.SetBool("IsWalking", false);
    }
}