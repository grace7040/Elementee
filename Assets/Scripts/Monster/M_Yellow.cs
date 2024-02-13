using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Yellow : MonsterController
{

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (distanceX <= detectionRange && distanceY <= 1.0f)
        {
            Isfirst = true;

            // Flip
            if (distance < -0.1f)
            {
                m_sprite.flipX = false;
            }
            else if (distance > 0.1f)
            {
                m_sprite.flipX = true;
            }

            // Attack
            if (distanceX <= attackRange && distanceY <= 1.0f)
            {
                if (!CheckGround()) { }
                else
                {
                    animator.SetBool("IsWalking", false);

                    if (canAttack)
                    {
                        // Àá½Ã ¸ØÃè´Ù°¡ µ¹Áø
                        StartCoroutine(ChargeAfterDelay());
                        Color.Attack(this);
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
                        // Move
                        animator.SetBool("IsWalking", true);
                        Vector2 moveDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
                        rb.velocity = moveDirection * moveSpeed;
                    }
                }
            }
        }
        else
        {
            if (canAttack)
            {
                if (Isfirst)
                {
                    SetWaypoints();
                    currentWaypoint = waypoint_L;
                    m_sprite.flipX = false;
                    Isfirst = false;
                }

                timer += Time.deltaTime;

                if (timer >= interval)
                {
                    SetWaypoints();
                    timer = 0.0f;
                }

                if (isKnockedBack) { }
                else
                {
                    if (currentWaypoint != null)
                    {
                        MoveTowardsWaypoint();
                    }
                }
            }
        }
    }
}
