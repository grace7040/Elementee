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

    /*Colors�� ���� ���� ��ȯ�ϴ� �Լ�. ���� ���� �� ������ ���⼭��. */
    public Color GetColor(Colors color)
    {
        switch (color)
        {
            case Colors.def:
                return Color.black;
            case Colors.red:
                return Color.red;
            case Colors.yellow:
                return Color.yellow;
            case Colors.blue:
                return Color.blue;
            case Colors.orange:
                return new Color32(255, 127, 0, 255);
            case Colors.green:
                return Color.green;
            case Colors.purple:
                return new Color32(128, 65, 127, 255);
            case Colors.black:
                return Color.black;
            default:
                return Color.black;
        }
    }


    /*�÷��̾��� Color�� ������ �� ������ �� �Լ��� ���� �����ؾ� ��.*/
    public void SetColorState(Colors _color)
    {
        player = FindObjectOfType<PlayerController>();
        player.myColor = _color;

        // ���ο� �� ����� �� ���� �׸����� UI ����
        if (!colorList.Contains(_color))
            StartDrawing(_color);

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

    private void SetColorState(IColorState _color)
    {
        player.Color = _color;
    }

    public void StartDrawing(Colors _color)
    {
        // ���� �Ͻ�����
        GameManager.Instance.PauseGame();

        Managers.UI.ShowPopupUI<UI_DrawCanvas>();
        colorList.Add(_color);

        DrawManager.Instance.SetBrushColor(_color);
        DrawManager.Instance.OpenDrawing();
    }


    public void SetPlayerCustomWeapon()
    {
        player.SetCustomWeapon();
        OnSaveColor?.Invoke();
    }
    
}
