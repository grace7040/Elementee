using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public SpriteRenderer sr;
    public float width = 0.25f;
    public float height = 0.33333f;

    private void Start()
    {
        GetComponent<Transform>().localScale = new Vector3(sr.sprite.rect.width* width, sr.sprite.rect.height* height, 0);
    }
}
