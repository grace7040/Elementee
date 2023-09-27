using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;

public class DrawManager : Singleton<DrawManager>
{
    public GameObject DrawbleObject;
    Colors color;
    public Sprite[] sprites;

    [HideInInspector]
    public DrawingSettings DrawSetting;

    private void Awake()
    {
        DrawSetting = GetComponentInChildren<DrawingSettings>();
    }
    public void SetBrushColor(Colors color)
    {
        this.color = color;
        Color c = new Color(0, 0, 0);

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        switch (color)
        {
            case Colors.def:
                c = Color.black;
                break;
            case Colors.red:
                c = Color.red;
                break;
        }

        DrawSetting.SetMarkerColour(c);
    }

    public void OpenDrwableObject()
    {
        DrawbleObject.SetActive(true);
    }

    public void CloseDrawbleObject()
    {
        DrawbleObject.SetActive(false);
    }
}
