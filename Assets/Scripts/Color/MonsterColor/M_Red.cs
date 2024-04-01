using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Red : MonsterController
{
    public GameObject fireObject;

    new void Update()
    {
        base.Update();
        if (isDie) return;

        if (distanceX <= detectionRange && distanceY <= 1.0f)
        {
            if (CheckGround())
            {
                if (CheckCliff())
                {
                    rb.velocity = Vector2.zero;
                }
            }
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
                        Attack();

                        canAttack = false;
                        canflip = false;

                        this.CallOnDelay(2.0f, () => {
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
                        Vector2 moveDirection = (currentWaypoint - transform.position);
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
                if (!isGrounded && distanceX <= detectionRange)
                {
                    if (CheckCliff())
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            currentWaypoint = player.position;
                            Vector2 moveDirection = (currentWaypoint - transform.position);
                            moveDirection.y = 0;
                            moveDirection.Normalize();
                            rb.velocity = moveDirection * moveSpeed;
                        }
                    }
                }
                else
                {
                    if (canAttack) Move();
                }
            }
        }
    }

    void Attack()
    {
        fireObject.SetActive(true);
        this.CallOnDelay(1f, () => fireObject.SetActive(false));
    }
}
