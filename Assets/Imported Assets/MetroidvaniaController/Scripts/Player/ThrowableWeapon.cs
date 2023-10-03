using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
	//public bool hasHit = false;
	//public float speed = 10f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }


    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
    //        Destroy(gameObject);
    //    }
    //    else if (collision.gameObject.tag != "Player")
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
