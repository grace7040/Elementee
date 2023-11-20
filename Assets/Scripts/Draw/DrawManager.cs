using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;

public class DrawManager : Singleton<DrawManager>
{
    public GameObject Drawing;
    public GameObject DrawbleObject;
    public GameObject Cam;
    public GameObject DrawCam;

    Colors color;
    public Sprite[] sprites;

    [HideInInspector]
    public DrawingSettings DrawSetting;

    private void Awake()
    {
        Cam = GameObject.Find("Camera");
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
        // Draw용 카메라로
        Cam.SetActive(false);
        DrawCam.SetActive(true);

        Drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        // 다시 원래 카메라로
        Cam.SetActive(true);
        DrawCam.SetActive(false);

        Drawing.SetActive(false);
    }

    public void SetFaceColor()
    {
        Color c = ColorManager.Instance.GetColor(Colors.black);

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(22);

        DrawSetting.SetMarkerColour(c);

    }

    public void SaveFaceDrawing()
    {
        // 다시 원래 카메라로
        Cam.SetActive(true);
        DrawCam.SetActive(false);

        Drawing.SetActive(false);
        Managers.UI.ShowPopupUI<UI_Custom>();

        // Face 저장
        GameManager.Instance.playerFace = sprites[0];
    }
}
