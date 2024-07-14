using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(MonsterController monster) : base(monster) { }

    public override void Enter()
    {
        monster.Rb.velocity = Vector2.zero;
        monster.Animator.SetBool("IsAttacking", true);
        monster.StartCoroutine(AttackRoutine());
    }

    public override void Update()
    {
        if (monster.CanFlip)
        {
            monster.IsFlip = monster.Player.position.x < monster.transform.position.x;
            monster.MonsterBody.rotation = monster.IsFlip ? Quaternion.identity : monster.FlipQuaternion;
        }
    }

    public override void Exit()
    {
        monster.Animator.SetBool("IsAttacking", false);
    }

    private IEnumerator AttackRoutine()
    {
        if (monster.MyColor == Colors.Blue)
        {
            monster.CanFlip = false;
            yield return new WaitForSeconds(1f);
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2.0f);
                monster.CanFlip = true;
                monster.ChangeState(new ChaseState(monster));
            }
        }
        else if (monster.MyColor == Colors.Red)
        {
            monster.CanFlip = false;
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2f);
                monster.CanFlip = true;
                monster.ChangeState(new IdleState(monster));
            }
        }
        else if (monster.MyColor == Colors.Yellow)
        {
            monster.CanFlip = false;
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2f);
                monster.CanFlip = true;
                monster.ChangeState(new IdleState(monster));
            }
        }
    }
}