using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    public Colors doorColor;
    BoxCollider2D boxcollider;
    private Animator anim;

    public int force = 100;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(doorColor);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().myColor == doorColor)
            {
                boxcollider.enabled = false;
                return;
            }  
            else
            {
                float dir = collision.transform.position.x - transform.position.x;
                collision.rigidbody.AddForce(new Vector2(dir, 0) * force, ForceMode2D.Impulse);

                if (dir>0)
                    anim.Play("Door_R", -1, 0.2f);
                else
                    anim.Play("Door_L", -1, 0.2f);

            }
        }
        boxcollider.enabled = true;
    }



}
