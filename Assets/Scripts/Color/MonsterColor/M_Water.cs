using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Water : PoolAble
{
    public Vector3 direction;
    public float speed = 10f;
    Rigidbody2D rb;

    bool m_isReleased;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 방향 처리 필요
        //direction = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
        //Destroy(gameObject, 2.0f);
        
    }

    private void OnEnable()
    {
        m_isReleased = false;
        this.CallOnDelay(2f, () => { if (m_isReleased) return; else { m_isReleased = true;  ReleaseObject(); } });
    }
    private void FixedUpdate()
    {
        //direction.y = 0;
        rb.velocity = direction.normalized * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            //Destroy(gameObject);
            m_isReleased = true;
            ReleaseObject();
        }
    }

}
