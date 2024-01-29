using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFountain : MonoBehaviour
{
    public Colors fountainColor;
    BoxCollider2D boxcollider;
    public int damage = 5;
    public int healAmount = 5;
    PlayerController player;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(fountainColor);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("OK1");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("OK2");
            player = collision.gameObject.GetComponent<PlayerController>();
            if (player.myColor == fountainColor)
            {
                Debug.Log("OK3");
                player.HealWithFountain(healAmount);
            }
            else
            {
                player.TakeDamage(damage, this.transform.position);
            }
        }
    }

}