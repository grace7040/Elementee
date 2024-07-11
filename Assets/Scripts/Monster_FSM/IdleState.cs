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
            if (monster.CheckGround())
            {
                monster.Move();
            }
            else
            {
                monster.Rb.velocity += Time.deltaTime * Vector2.down;
            }
        }
        else
        {
            if (Vector2.Distance(monster.transform.position, monster.Player.position) <= monster.DetectionRange && monster.DistanceY <= 1.0f)
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
                    monster.Rb.velocity += Time.deltaTime * Vector2.down;
                }
            }
        }
    }

    public override void Exit() { }
}