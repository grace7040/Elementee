using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public List<GameObject> Platforms = new List<GameObject>(2);
    public float fallDelay = 0f;

    BoxCollider2D boxcollider;
    Rigidbody2D rigid0;
    Rigidbody2D rigid1;
    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        rigid0 = Platforms[0].transform.GetComponent<Rigidbody2D>();
        rigid1 = Platforms[1].transform.GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            boxcollider.enabled = false;
            this.CallOnDelay(fallDelay, ()=>{ rigid0.constraints = RigidbodyConstraints2D.None; });
            this.CallOnDelay(fallDelay, ()=>{ rigid1.constraints = RigidbodyConstraints2D.None; });
        }
    }
}
