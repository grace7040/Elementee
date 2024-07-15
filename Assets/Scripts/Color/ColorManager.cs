using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController _player;
    PlayerAttack _playerAttack;
    List<Colors> colorList = new();

    public Action OnSetColor = null;
    public Action OnSaveColor = null;
    public Action<string, bool> SetPlayerAnimatorBool = null;

    public bool basicWeapon = false;


    [Header("Color State")]
    [SerializeField]
    private bool hasRed = false;
    [SerializeField]
    private bool hasBlue = false;
    [SerializeField]
    private bool hasYellow = true;

    public bool HasRed
    {
        get { return hasRed; }
        set
        {
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
        colorList.Add(Colors.Default);
    }

    public void InitPlayer(PlayerController player, Action<string, bool> setAnimBoolAction)
    {
        _player = player;
        SetPlayerAnimatorBool = setAnimBoolAction;
    }

    public void InitPlayerAttack(PlayerAttack playerAttack)
    {
        _playerAttack = playerAttack;
    }
    public void ResetColorState()
    {
        hasYellow = false;
        hasRed = false;
        hasBlue = false;
    }

    /*Colors별 실제 색상값 반환하는 함수. 색상값 설정 및 변경은 여기서만. */
    public Color GetColor(Colors color)
    {
        return color switch
        {
            Colors.Default => Color.white,
            Colors.Red => new Color32(254, 120, 120, 255),
            Colors.Yellow => new Color32(255, 229, 73, 255),
            Colors.Blue => new Color32(133, 151, 255, 255),
            Colors.Orange => new Color32(255, 175, 61, 255),
            Colors.Green => new Color32(111, 189, 111, 255),
            Colors.Purple => new Color32(153, 93, 227, 255),
            Colors.Black => Color.black,
            _ => Color.black,
        };
    }


    /*플레이어의 Color을 변경할 시 무조건 이 함수를 통해 변경해야 함.*/
    public void SetColorState(Colors _color)
    {
        //player = FindObjectOfType<PlayerController>();
        _player.myColor = _color;
        GameManager.Instance.playerColor = _color;

        // 새로운 색 사용할 때 무기 그리도록 UI 띄우기
        if (!colorList.Contains(_color) && _color != Colors.Black)
        {
            OffPlayerWeapon();
            StartDrawing(_color);
        }
        else
            SetOnPlayer(_color);

    }


    // hasXXX 변수 설정 & 플레이어 무기 활성화
    public void SetOnPlayer(Colors _color)
    {
        _playerAttack.canAttack = true;

        switch (_color)
        {
            case Colors.Default:
                //hasRed = false;
                //hasYellow = false;
                //hasBlue = false;
                SetColorState(new DefaultColor(SetPlayerAnimatorBool));
                break;
            case Colors.Red:
                hasRed = false;
                SetColorState(new RedColor(SetPlayerAnimatorBool));
                _playerAttack.RedWeapon.SetActive(true);
                break;
            case Colors.Yellow:
                hasYellow = false;
                _playerAttack.canAttack = false;
                SetColorState(new YellowColor());
                _playerAttack.YellowWeaponEffect.SetActive(true);
                break;
            case Colors.Blue:
                hasBlue = false;
                SetColorState(new BlueColor(SetPlayerAnimatorBool));
                //player.BlueWeapon.SetActive(true);
                break;
            case Colors.Green:
                hasYellow = false;
                hasBlue = false;
                SetColorState(new GreenColor(SetPlayerAnimatorBool));
                //player.GreenWeapon.SetActive(true);
                break;
            case Colors.Purple:
                hasRed = false;
                hasBlue = false;
                SetColorState(new PurpleColor(SetPlayerAnimatorBool));
                _playerAttack.PurpleWeapon.SetActive(true);
                break;
            case Colors.Orange:
                hasYellow = false;
                hasRed = false;
                SetColorState(new OrangeColor());
                break;
            case Colors.Black:
                hasYellow = false;
                hasRed = false;
                hasBlue = false;
                _playerAttack.BlackWeapon.SetActive(true);
                SetColorState(new BlackColor());
                break;
        }
        _player.myColor = _color;

        // 기본 무기 or 커스텀 무기 적용
        if (basicWeapon)
            SetPlayerBasicWeapon();
        else
            SetPlayerCustomWeapon();

        ObjectPoolManager.Instance.SetColorName(_color);
        DrawManager.Instance.SaveWeapon((int)_color);
        OnSetColor?.Invoke();
    }

    private void SetColorState(IColorState _color)
    {
        _player.Color = _color;
        _playerAttack.RedWeapon.SetActive(false);
        _playerAttack.YellowWeaponEffect.SetActive(false);
        _playerAttack.PurpleWeapon.SetActive(false);
        _playerAttack.GreenWeapon.SetActive(false);
        _playerAttack.BlueWeapon.SetActive(false);
    }

    private void OffPlayerWeapon()
    {
        _playerAttack.RedWeapon.SetActive(false);
        _playerAttack.YellowWeaponEffect.SetActive(false);
        _playerAttack.PurpleWeapon.SetActive(false);
        _playerAttack.GreenWeapon.SetActive(false);
        _playerAttack.BlueWeapon.SetActive(false);
        _playerAttack.BlackWeapon.SetActive(false);
    }

    public void StartDrawing(Colors _color)
    {
        // 게임 일시정지
        GameManager.Instance.PauseGame();

        Managers.UI.ShowPopupUI<UI_DrawCanvas>();
        colorList.Add(_color);

        DrawManager.Instance.SetBrushColor(_color);
        DrawManager.Instance.OpenDrawing();
    }


    public void SetPlayerCustomWeapon()
    {
        basicWeapon = false;
        _playerAttack.SetCustomWeapon();
        OnSaveColor?.Invoke();
    }

    public void SetPlayerBasicWeapon()
    {
        basicWeapon = true;
        _playerAttack.SetBasicWeapon();
        OnSaveColor?.Invoke();
    }

}