using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFountain : MonoBehaviour
{
    public Colors fountainColor;
    BoxCollider2D boxcollider;
    int damage = 10;
    int healAmount = 10;
    PlayerController player;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(fountainColor);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            if (player.myColor == fountainColor)
                player.HealWithFountain(healAmount);
            else
                player.TakeDamage(damage, this.transform.position);

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<MonsterController>().myColor != fountainColor)
            {
                collision.gameObject.GetComponent<MonsterController>().TakeDamage(damage, this.transform.position);
            }
        }
    }

    // ¿Ã∆Â∆Æ √ﬂ∞°
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ¿Ã∆Â∆Æ √ﬂ∞°
        if (collision.gameObject.CompareTag("Player"))
        {
            var bulletGo = ObjectPoolManager.Instance.GetGo(fountainColor);
            bulletGo.transform.position = transform.position;
        }
    }


}