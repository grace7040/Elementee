using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;

public class DrawManager : Singleton<DrawManager>
{
    public GameObject Drawing;
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
        Color c = ColorManager.Instance.GetColor(color);

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerColour(c);
    }

    public void OpenDrawing()
    {
        Drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        Drawing.SetActive(false);
    }
}
