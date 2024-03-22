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
                if (CheckGround())
                {
                    if (canAttack)
                    {
                        rb.velocity = Vector2.zero;
                        currentWaypoint = player.position;
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("IsAttacking", true);

                        this.CallOnDelay(1f, () => { if (!isDie) Attack(); });

                        canAttack = false;
                        canflip = false;

                        this.CallOnDelay(3f, () =>
                        {
                            animator.SetBool("IsAttacking", false);
                            canAttack = true;
                            canflip = true;
                        });
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
                        Vector2 moveDirection = (player.position - transform.position);
                        moveDirection.y = 0;
                        moveDirection.Normalize();
                        rb.velocity = moveDirection * moveSpeed;
                    }
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
                if (!isGrounded && distanceX <= detectionRange) currentWaypoint = player.position;
                else
                {
                    if (canAttack) Move();
                }
            }
        }
    }

    void Attack()
    {
        Water = ObjectPoolManager.Instance.GetGo("MonsterWater");
        Water.transform.position = transform.position;
        Water.GetComponent<M_Water>().direction = isFlip ? new Vector3(-1, 0,0): new Vector3(1, 0, 0);
    }
}
