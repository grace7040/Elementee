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
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    collision.rigidbody.velocity = Vector2.zero;

        //    dir = collision.transform.position - transform.position;
        //    dir = dir.normalized;
        //    collision.rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
        //}

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().myColor == doorColor)
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

                // float dir = collision.transform.position.x - transform.position.x;
                //  collision.rigidbody.velocity = new Vector2(dir * force, collision.rigidbody.velocity.y);
                //   collision.rigidbody.AddForce(new Vector2(dir, 0) * force, ForceMode2D.Impulse);

                //if (dir.x > 0 || dir.y < 0)
                //    anim.Play("Door_R", -1, 0.2f);
                //else
                //    anim.Play("Door_L", -1, 0.2f);
            }
        }
        boxcollider.enabled = true;
    }



}
