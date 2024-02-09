using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGround : MonoBehaviour
{
    public Colors groundColor;
    BoxCollider2D boxcollider;

    int damage = 30;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(groundColor);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().myColor != groundColor)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage, this.transform.position);
            }
        }
        

    }

}
