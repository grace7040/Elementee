using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : PoolAble
{
	public Vector2 direction;
    public Colors myColor;
    //public bool hasHit = false;
    public float speed = 20f;
    private bool isReleased;
    //Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        //rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = DrawManager.Instance.sprites[(int)myColor];
    }

    private void OnEnable()
    {
        isReleased = false;
        this.CallOnDelay(3f, ReleaseThrowableWeapon);
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
        //rigid.AddForce(direction, ForceMode2D.Impulse);
        transform.Translate(speed * Time.deltaTime * direction);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!collision.gameObject.GetComponent<PlayerController>())
    //    {
    //        //Destroy(gameObject);
    //        ReleaseObject();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            //Destroy(gameObject);
            ReleaseThrowableWeapon();
            //isReleased = true;
        }
    }

    public void Throw(Vector3 playerPosition, float playerLocalScaleX)
    {
        if (ColorManager.Instance.basicWeapon)
            SetBasicWeapon();
        spriteRenderer.flipX = playerLocalScaleX < 0;
        transform.SetPositionAndRotation(playerPosition + new Vector3(playerLocalScaleX * 0.5f, 0.2f), Quaternion.identity);
        direction = new Vector2(playerLocalScaleX, 0);
    }

    void ReleaseThrowableWeapon()
    {
        if (isReleased) return;

        ReleaseObject();
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
