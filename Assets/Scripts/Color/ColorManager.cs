using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController _playerController;
    PlayerAttack _playerAttack;
    List<Colors> _colorList = new();

    //Delegetes
    public Action OnSetColor = null;
    public Action OnSaveColor = null;
    public Action<string, bool> SetPlayerAnimatorBool = null;
    public Action ShakeCamera = null;
    public Action<float> OnOrangeAttacked = null;
    public Action OnYellowAttacked = null;
    public Action OnBlackAttacked = null;
    public Action OnSetBlackColor = null;


    public bool IsUsingBasicWeapon = false;


    [Header("Color State")]
    [SerializeField]
    bool _hasRed = false;
    [SerializeField]
    bool _hasBlue = false;
    [SerializeField]
    bool _hasYellow = false;

    public bool HasRed
    {
        get { return _hasRed; }
        set
        {
            _hasRed = value;
            OnSetColor?.Invoke();
        }
    }

    public bool HasBlue
    {
        get { return _hasBlue; }
        set
        {
            _hasBlue = value;
            OnSetColor?.Invoke();
        }
    }

    public bool HasYellow
    {
        get { return _hasYellow; }
        set
        {
            _hasYellow = value;
            OnSetColor?.Invoke();
        }
    }

    private void Awake()
    {
        _colorList.Add(Colors.Default);
    }

    public void InitPlayer(PlayerController player, Action<string, bool> setAnimBoolAction, Action shakeCameraAction)
    {
        _playerController = player;
        SetPlayerAnimatorBool = setAnimBoolAction;
        ShakeCamera = shakeCameraAction;
    }

    public void InitPlayerAttack(PlayerAttack playerAttack, Action<float> onOrangeAttackedAction, Action onYellowAttackedAction, Action onBlackAttackedAction, Action onSetBlackAction)
    {
        _playerAttack = playerAttack;
        OnOrangeAttacked = onOrangeAttackedAction;
        OnYellowAttacked = onYellowAttackedAction;
        OnBlackAttacked = onBlackAttackedAction;
        OnSetBlackColor = onSetBlackAction;
    }
    public void ResetColorState()
    {
        _hasYellow = false;
        _hasRed = false;
        _hasBlue = false;
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
        _playerController.myColor = _color;
        GameManager.Instance.PlayerColor = _color;

        // 새로운 색 사용할 때 무기 그리도록 UI 띄우기
        if (!_colorList.Contains(_color) && _color != Colors.Black)
        {
            OffPlayerWeapon();
            StartDrawing(_color);
        }
        else
            SetPlayerColorState(_color);

    }


    // hasXXX 변수 설정 & 플레이어 무기 활성화
    void SetPlayerColorState(Colors _color)
    {
        _playerAttack.canAttack = true;

        switch (_color)
        {
            case Colors.Default:
                SetColorState(new DefaultColor(SetPlayerAnimatorBool));
                break;
            case Colors.Red:
                SetColorState(new RedColor(SetPlayerAnimatorBool));
                _playerAttack.RedWeapon.SetActive(true);
                break;
            case Colors.Yellow:
                _playerAttack.canAttack = false;
                SetColorState(new YellowColor(OnYellowAttacked));
                _playerAttack.YellowAttackEffect.SetActive(true);
                break;
            case Colors.Blue:
                _playerAttack.BlueWeapon.SetActive(true);
                SetColorState(new BlueColor(SetPlayerAnimatorBool));
                break;
            case Colors.Green:
                _playerAttack.GreenWeapon.SetActive(true);
                SetColorState(new GreenColor(SetPlayerAnimatorBool));
                break;
            case Colors.Purple:
                SetColorState(new PurpleColor(SetPlayerAnimatorBool, ShakeCamera));
                _playerAttack.PurpleWeapon.SetActive(true);
                break;
            case Colors.Orange:
                SetColorState(new OrangeColor(OnOrangeAttacked));
                break;
            case Colors.Black:
                _playerAttack.BlackWeapon.SetActive(true);
                SetColorState(new BlackColor(OnBlackAttacked));
                OnSetBlackColor.Invoke();
                break;
        }
        //_playerController.myColor = _color;

        UseBasicWeapon(IsUsingBasicWeapon);

        ObjectPoolManager.Instance.SetColorName(_color);
        DrawManager.Instance.SaveWeapon((int)_color);
        OnSetColor?.Invoke();
    }

    void SetColorState(IColorState _color)
    {
        _playerController.Color = _color;
        _playerAttack.RedWeapon.SetActive(false);
        _playerAttack.YellowAttackEffect.SetActive(false);
        _playerAttack.PurpleWeapon.SetActive(false);
    }

    void OffPlayerWeapon()
    {
        _playerAttack.RedWeapon.SetActive(false);
        _playerAttack.YellowAttackEffect.SetActive(false);
        _playerAttack.PurpleWeapon.SetActive(false);
        _playerAttack.GreenWeapon.SetActive(false);
        _playerAttack.BlueWeapon.SetActive(false);
        _playerAttack.BlackWeapon.SetActive(false);
    }

    public void StartDrawing(Colors _color)
    {
        // 게임 일시정지
        GameManager.Instance.PauseGame();

        UIManager.Instance.ShowPopupUI<UI_DrawCanvas>();
        _colorList.Add(_color);

        DrawManager.Instance.SetBrushColor(_color);
        DrawManager.Instance.OpenDrawing();
    }

    public void UseBasicWeapon(bool value)
    {
        IsUsingBasicWeapon = value;
        if (value)
            _playerAttack.SetBasicWeapon();
        else
            _playerAttack.SetCustomWeapon();

        OnSaveColor?.Invoke();
    }

}