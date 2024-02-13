using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Blue : MonsterController
{
    GameObject Water;

    void Update()
    {
        base.Update();
        if (isDie) return;
        if (distanceX <= detectionRange && distanceY <= 1.0f)
        {
            // Attack
            if (distanceX <= attackRange && distanceY <= 1.0f)
            {
                if (!CheckGround()) { }
                else
                {
                    if (canAttack)
                    {
                        animator.SetBool("IsWalking", false);
                        rb.velocity = Vector2.zero;
                        animator.SetBool("IsAttacking", true);
                        this.CallOnDelay(1f, () => {
                            animator.SetBool("IsAttacking", true);
                            Attack();
                        });
                        StartCoroutine(AttackCooldown_B());
                    }
                }
            }
            else
            {
                if (!CheckGround())
                {
                    rb.velocity += Time.deltaTime * Vector2.down;
                }
                else
                {
                    if (canAttack)
                    {
                        currentWaypoint = player.position;
                        // Move
                        animator.SetBool("IsWalking", true);
                        Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                        rb.velocity = moveDirection * moveSpeed;
                    }
                }
            }
        }
        else
            Move();
    }

    void Attack()
    {

        //Water = Instantiate(Resources.Load("Monster/Waters"), transform.position, Quaternion.identity) as GameObject;
        Water = ObjectPoolManager.Instance.GetGo("MonsterWater");
        Water.transform.position = transform.position;
        Water.GetComponent<M_Water>().direction = isFlip ? new Vector3(-1, 0,0): new Vector3(1, 0, 0);

    }
}
