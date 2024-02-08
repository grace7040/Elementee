using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public List<GameObject> Platforms = new List<GameObject>();
    public float fallDelay = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            this.CallOnDelay(fallDelay, ()=>{ Platforms[0].transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; });
            this.CallOnDelay(fallDelay, ()=>{ Platforms[1].transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; });
        }
    }
}
