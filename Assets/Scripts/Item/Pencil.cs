using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : MonoBehaviour
{
    BoxCollider2D boxcollider;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.ReDrawItemColor = collision.GetComponent<PlayerController>().myColor;
            Destroy(this.gameObject);
        }
    }

}
