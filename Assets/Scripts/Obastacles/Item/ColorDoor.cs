using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    public Colors doorColor;
    public BoxCollider2D boxcollider;
    private Animator anim;

    private Vector2 dir = new Vector2(0,0);
    public int force = 100;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(doorColor);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().MyColor == doorColor)
            {
                boxcollider.enabled = false;
                return;
            }
            else
            {
                AudioManager.Instance.PlaySFX("ColorDoor");

                collision.rigidbody.velocity = Vector2.zero;

                dir = collision.transform.position - transform.position;
                dir = dir.normalized;
                collision.rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<MonsterController>().MyColor == doorColor)
            {
                boxcollider.enabled = false;
                return;
            }
            else
            {
                collision.rigidbody.velocity = Vector2.zero;

                dir = collision.transform.position - transform.position;
                dir = dir.normalized;
                collision.rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }
    }



}
