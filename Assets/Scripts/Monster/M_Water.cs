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
    }

    private void OnEnable()
    {
        m_isReleased = false;
        this.CallOnDelay(2f, () => { if (m_isReleased) return; else { m_isReleased = true;  ReleaseObject(); } });
    }
    private void FixedUpdate()
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Player"))
        {
            m_isReleased = true;
            ReleaseObject();
        }
    }

}
