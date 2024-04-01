using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Yellow : MonsterController
{
    public GameObject voltObject;

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
                    animator.SetBool("IsWalking", false);
                    currentWaypoint = player.position;

                    if (canAttack)
                    {
                        // Àá½Ã ¸ØÃè´Ù°¡ µ¹Áø
                        StartCoroutine(ChargeAfterDelay());
                        Attack();
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

    void Attack()
    {
        voltObject.SetActive(true);
        this.CallOnDelay(1f, () => voltObject.SetActive(false));
    }
}
