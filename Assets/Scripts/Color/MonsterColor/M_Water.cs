using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Water : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 10f;

    private void Start()
    {
        direction = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
        Destroy(gameObject, 2.0f);
    }
    private void FixedUpdate()
    {
        //direction = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
        direction.y = 0;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
}
