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
        // 공격 중 flip 제한
        //if (monster.CanFlip)
        //{
        //    monster.IsFlip = monster.player.position.x < monster.transform.position.x;
        //    monster.MonsterBody.rotation = monster.IsFlip ? Quaternion.identity : monster.FlipQuaternion;
        //}
        monster.IsFlip = monster.Player.position.x < monster.transform.position.x;
        monster.MonsterBody.rotation = monster.IsFlip ? Quaternion.identity : monster.FlipQuaternion;
    }

    public override void Exit()
    {
        monster.Animator.SetBool("IsAttacking", false);
    }

    private IEnumerator AttackRoutine()
    {
        if (monster.MyColor == Colors.Blue)
        {
            yield return new WaitForSeconds(1f);
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2f);
                monster.ChangeState(new IdleState(monster));
            }
        }
        else if (monster.MyColor == Colors.Red)
        {
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2f);
                monster.ChangeState(new IdleState(monster));
            }
        }
        else if (monster.MyColor == Colors.Yellow)
        {
            if (!monster.IsDie)
            {
                monster.Attack();
                yield return new WaitForSeconds(2f);
                monster.ChangeState(new IdleState(monster));
            }
        }
    }
}