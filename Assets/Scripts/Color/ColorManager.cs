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
    [SerializeField]
    private bool hasRed = false;
    [SerializeField]
    private bool hasBlue = false;
    [SerializeField]
    private bool hasYellow = false;

    public bool HasRed
    {
        get { return hasRed; }
        set { 
            hasRed = value;
            OnSetColor?.Invoke();
        }
    }

    public bool HasBlue
    {
        get { return hasBlue; }
        set
        {
            hasBlue = value;
            OnSetColor?.Invoke();
        }
    }

    public bool HasYellow
    {
        get { return hasYellow; }
        set
        {
            hasYellow = value;
            OnSetColor?.Invoke();
        }
    }

    private void Awake()
    {
        colorList.Add(Colors.def);
    }

    public void ResetColorState()
    {
        hasYellow = false;
        hasRed = false;
        hasBlue = false;
    }

    /*Colors�� ���� ���� ��ȯ�ϴ� �Լ�. ���� ���� �� ������ ���⼭��. */
    public Color GetColor(Colors color)
    {
        switch (color)
        {
            case Colors.def:
                return Color.white;
            case Colors.red:
                return new Color32(254, 120, 120, 255); 
            case Colors.yellow:
                return new Color32(255, 229, 73, 255);
            case Colors.blue:
                return new Color32(133, 151, 255, 255);
            case Colors.orange:
                return new Color32(255, 175, 61, 255); 
            case Colors.green:
                return new Color32(109, 201, 109, 255);
            case Colors.purple:
                return new Color32(167, 54, 200, 255);
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
        GameManager.Instance.playerColor = _color;
       

        // ���ο� �� ����� �� ���� �׸����� UI ����
        if (!colorList.Contains(_color))
            StartDrawing(_color);

        switch (_color)
        {
            case Colors.def:
                //hasRed = false;
                //hasYellow = false;
                //hasBlue = false;
                SetColorState(new DefaultColor());
                break;
            case Colors.red:
                hasRed = false;
                SetColorState(new RedColor());
                player.red_Weapon.SetActive(true);
                break;
            case Colors.yellow:
                hasYellow = false;
                SetColorState(new YellowColor());
                break;
            case Colors.blue:
                hasBlue = false;
                SetColorState(new BlueColor());
                player.blue_Weapon.SetActive(true);
                break;
            case Colors.green:
                hasYellow = false;
                hasBlue = false;
                SetColorState(new GreenColor());
                player.green_Weapon.SetActive(true);
                break;
            case Colors.purple:
                hasRed = false;
                hasBlue = false;
                SetColorState(new PurpleColor());
                player.purple_Weapon.SetActive(true);
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
        ObjectPoolManager.Instance.SetColorName(_color);
        OnSetColor?.Invoke();
    }

    private void SetColorState(IColorState _color)
    {
        player.Color = _color;
        player.red_Weapon.SetActive(false);
        player.purple_Weapon.SetActive(false);
        player.green_Weapon.SetActive(false);
        player.blue_Weapon.SetActive(false);
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
