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
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerController>().myColor == Colors.def) 
                return;

            AudioManager.Instacne.PlaySFX("Sponge");
            var bulletGo = ObjectPoolManager.Instance.GetGo();
            bulletGo.transform.position = transform.position;

            GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(collision.GetComponent<PlayerController>().myColor);
            ColorManager.Instance.SetColorState(Colors.def);

            // 애니메이션 재생
            anim.Play("Sponge", -1, 0f);

        }
    }

}
