using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
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
            if (collision.GetComponent<PlayerController>().myColor == Colors.def) 
                return;

            GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(collision.GetComponent<PlayerController>().myColor);
            ColorManager.Instance.SetColorState(Colors.def);
        }
    }

}
