using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGround : MonoBehaviour
{
    public Colors groundColor;

    int damage = 30;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(groundColor);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().MyColor != groundColor)
            {
                collision.GetComponent<PlayerController>().TakeDamage(damage, this.transform.position);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<MonsterController>().MyColor != groundColor)
            {
                collision.GetComponent<MonsterController>().TakeDamage(damage, this.transform.position);
            }
        }
    }

}
