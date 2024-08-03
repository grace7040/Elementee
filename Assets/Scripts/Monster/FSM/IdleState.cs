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
        monster.Animator.SetBool("IsWalking", false);
    }

    public override void Update()
    {
        if (monster.MyColor == Colors.Default)
        {
            MoveOnGround();
        }
        else
        {
            if (Vector2.Distance(monster.transform.position, monster.Player.position) <= monster.DetectionRange && monster.DistanceY <= 0.7f)
            {
                monster.ChangeState(new ChaseState(monster));
            }
            else
            {
                MoveOnGround();
            }
        }
    }

    public override void Exit() { }

    private void MoveOnGround()
    {
        if (monster.CheckGround())
        {
            monster.Move();
        }
        else
        {
            monster.Rb.velocity += Time.deltaTime * Vector2.down;
        }
    }
}