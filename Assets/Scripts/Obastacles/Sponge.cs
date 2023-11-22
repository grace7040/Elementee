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

            AudioManager.Instacne.PlaySFX("Sponge");
            var bulletGo = ObjectPoolManager.Instance.GetGo();
            bulletGo.transform.position = transform.position;

            GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(collision.GetComponent<PlayerController>().myColor);
            ColorManager.Instance.SetColorState(Colors.def);

        }
    }

}
