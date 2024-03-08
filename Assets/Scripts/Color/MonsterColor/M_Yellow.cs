using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Yellow : MonsterController
{
    public GameObject voltObject;

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isDie) return;
        if (distanceX <= detectionRange)
        {
            // Attack
            if (distanceX <= attackRange)
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
                        Vector2 moveDirection = (player.position - transform.position);
                        moveDirection.y = 0;
                        moveDirection.Normalize();
                        rb.velocity = moveDirection * moveSpeed;
                    }
                }
            }
        }
        else
            if (canAttack) Move();
    }

    void Attack()
    {
        voltObject.SetActive(true);
        this.CallOnDelay(1f, () => { voltObject.SetActive(false); });
    }
}
