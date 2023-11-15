using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decect : MonoBehaviour
{
    public GameObject WeaponPosition;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        rb = collision.gameObject.AddComponent<Rigidbody2D>();
    //        collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Vector2 Direction = (WeaponPosition.transform.position - collision.transform.position).normalized;
    //        rb.velocity = Direction * 7.5f;
    //    }
    //}
}
