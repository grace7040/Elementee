using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : PoolAble
{
	public Vector2 direction;
    public Colors myColor;
    //public bool hasHit = false;
    //public float speed = 20f;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = DrawManager.Instance.sprites[(int)myColor];
    }

    public void SetCustomWeapon()
    {
        spriteRenderer.sprite = DrawManager.Instance.sprites[(int)myColor];
    }

    public void SetBasicWeapon()
    {
        spriteRenderer.sprite = DrawManager.Instance.Basic_Sprites[(int)myColor];
    }


    private void FixedUpdate()
    {
        //rigid.velocity = direction * speed;
        rigid.AddForce(direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
        {
            //Destroy(gameObject);
            ReleaseObject();
        }
    }

    public void Throw(Vector3 playerPosition, float playerLocalScaleX)
    {
        if (ColorManager.Instance.basicWeapon)
            SetBasicWeapon();
        spriteRenderer.flipX = playerLocalScaleX < 0 ? true : false;
        transform.position = playerPosition + new Vector3(playerLocalScaleX * 0.5f, 0.2f);
        transform.rotation = Quaternion.identity;
        direction = new Vector2(playerLocalScaleX, 0);
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
