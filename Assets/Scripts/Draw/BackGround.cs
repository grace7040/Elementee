using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public SpriteRenderer sr;
    private void Start()
    {
        GetComponent<Transform>().localScale = new Vector3(sr.sprite.rect.width*0.25f, sr.sprite.rect.height*0.33333f, 0);
    }
}
