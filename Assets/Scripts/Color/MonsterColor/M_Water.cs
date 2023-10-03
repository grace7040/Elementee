using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Water : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10f;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
}
