using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            this.CallOnDelay(fallDelay, ()=>{ gameObject.transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; });
        }
    }
}
