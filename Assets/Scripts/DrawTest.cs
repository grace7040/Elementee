using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTest : MonoBehaviour
{
    public GameObject canvas;
    public void SetTexture()
    {
        GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<SpriteRenderer>().sprite;
    }
}
