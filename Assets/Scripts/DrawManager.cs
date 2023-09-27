using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;

public class DrawManager : Singleton<DrawManager>
{
    public GameObject drawing;
    Colors color;
    public Sprite[] sprites;
    public void SetBrushColor(Colors color)
    {
        this.color = color;
        Color c = new Color(0, 0, 0);

        drawing.GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
        GetComponentInChildren<Drawable>().UpdateCanvas();
        switch (color)
        {
            case Colors.def:
                c = Color.black;
                break;
            case Colors.red:
                c = Color.red;
                break;
        }
        GetComponentInChildren<DrawingSettings>().SetMarkerColour(c);
    }
}
