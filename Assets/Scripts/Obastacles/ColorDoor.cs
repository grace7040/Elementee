using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    public Colors doorColor;
    BoxCollider2D boxcollider;

    public int force = 100;

    private void Start()
    {
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
                collision.rigidbody.AddForce(new Vector2(collision.transform.position.x - transform.position.x, 0) * force, ForceMode2D.Impulse);
            }
        }
        boxcollider.enabled = true;
    }



}
