using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController player;
    List<Colors> colorList = new List<Colors>();

    public Action OnSetColor = null;
    public Action OnSaveColor = null;

    [Header("Color State")]
    public bool hasRed = false;
    public bool hasBlue = false;
    public bool hasYellow = false;

    private void Start()
    {
        colorList.Add(Colors.def);
    }
    private void SetColorState(IColorState _color)
    {
        player = FindObjectOfType<PlayerController>();
        player.Color = _color;
    }

    public void SetColorState(Colors _color)
    {
        player = FindObjectOfType<PlayerController>();
        // player.Color = _color;

        if (!colorList.Contains(_color))
        {
            // 게임 일시정지
            GameManager.Instance.PauseGame();

            Managers.UI.ShowPopupUI<UI_DrawCanvas>();
            colorList.Add(_color);

            DrawManager.Instance.SetBrushColor(_color);
            DrawManager.Instance.OpenDrawing();

        }
        switch (_color)
        {
            case Colors.def:
                hasRed = false;
                hasYellow = false;
                hasBlue = false;
                SetColorState(new DefaultColor());
                break;
            case Colors.red:
                hasRed = false;
                SetColorState(new RedColor());
                break;
            case Colors.yellow:
                hasYellow = false;
                SetColorState(new YellowColor());
                break;
            case Colors.blue:
                hasBlue = false;
                SetColorState(new BlueColor());
                break;
            case Colors.green:
                hasYellow = false;
                hasBlue = false;
                SetColorState(new GreenColor());
                break;
            case Colors.purple:
                hasRed = false;
                hasBlue = false;
                SetColorState(new PurpleColor());
                break;
            case Colors.orange:
                hasYellow = false;
                hasRed = false;
                SetColorState(new OrangeColor());
                break;
            case Colors.black:
                hasYellow = false;
                hasRed = false;
                hasBlue = false;
                SetColorState(new BlackColor());
                break;
        }
        player.myColor = _color;
        OnSetColor?.Invoke();
    }

    public void SetPlayerCustomWeapon()
    {
        player.SetCustomWeapon();
        OnSaveColor?.Invoke();
    }
}
