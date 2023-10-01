using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTest : MonoBehaviour
{
    public GameObject drawCanvas;
    public void SetTexture()
    {
        GetComponent<SpriteRenderer>().sprite = drawCanvas.GetComponent<SpriteRenderer>().sprite;
    }
}
