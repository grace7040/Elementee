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
        Color c = Color.clear;

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        switch (color)
        {
            case Colors.def:
                break;
            case Colors.red:
                c = Color.red;
                break;
            case Colors.yellow:
                c = Color.yellow;
                break;
            case Colors.blue:
                c = Color.blue;
                break;
            case Colors.orange:
                c = new Color32(255, 127, 0, 255);
                break;
            case Colors.green:
                c = Color.green;
                break;
            case Colors.purple:
                c = new Color32(128, 65, 127, 255);
                break;
            case Colors.black:
                c = Color.black;
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
