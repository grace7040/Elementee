using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBar : MonoBehaviour
{
    BoxCollider2D boxcollider;

    void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Generator"))
        {
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Generator"))
        {
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
