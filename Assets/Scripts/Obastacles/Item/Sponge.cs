using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    BoxCollider2D boxcollider;
    private Animator anim;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        anim = this.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().MyColor == Colors.Default) 
                return;

            AudioManager.Instance.PlaySFX("Sponge");
            var bulletGo = ObjectPoolManager.Instance.GetCurrentColorBlood();
            bulletGo.transform.position = transform.position;

            GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(collision.GetComponent<PlayerController>().MyColor);
            ColorManager.Instance.SetColorState(Colors.Default);

            anim.Play("Sponge", -1, 0f);

        }
    }

}
