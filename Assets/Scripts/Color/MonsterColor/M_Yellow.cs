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
        voltObject.SetActive(true);
        this.CallOnDelay(1f, () => { voltObject.SetActive(false); });
        //GameObject volt = ObjectPoolManager.Instance.GetGo("Volt");
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //volt.transform.SetParent(rb.transform);
        //volt.transform.localPosition = Vector3.zero;
    }
}
