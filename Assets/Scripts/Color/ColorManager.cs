using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    Dictionary<Colors, bool> _hasColorDict = new();    // Red, Yellow, Blue
    PlayerAttack _playerAttack;

    public bool IsUsingBasicWeapon = false;
     
    //Delegetes
    public Action OnSetColor = null;
    public Action OnSaveColor = null;
    Action<string, bool> SetPlayerAnimatorBool = null;
    Action ShakeCamera = null;
    Action<float> OnOrangeAttacked = null;
    Action OnYellowAttacked = null;
    Action OnBlackAttacked = null;
    Action OnSetBlackColor = null;
    Action<Colors> SetPlayerColor = null;
    Action<IColorState> SetPlayerColorState = null;

    public bool HasRed
    {
        get { return _hasColorDict[Colors.Red]; }
    }
    public bool HasYellow
    {
        get { return _hasColorDict[Colors.Yellow]; }
    }
    public bool HasBlue
    {
        get { return _hasColorDict[Colors.Blue]; }
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        _hasColorDict.TryAdd(Colors.Red, false);
        _hasColorDict.TryAdd(Colors.Yellow, false);
        _hasColorDict.TryAdd(Colors.Blue, false);
    }

    public void InitPlayer(Action<Colors> setMyColorAction, Action<IColorState> setColorStateAction, Action<string, bool> setAnimBoolAction, Action shakeCameraAction)
    {
        SetPlayerColor = setMyColorAction;
        SetPlayerColorState = setColorStateAction;
        SetPlayerAnimatorBool = setAnimBoolAction;
        ShakeCamera = shakeCameraAction;
        SetColorState(Colors.Default);
    }

    public void InitPlayerAttack(PlayerAttack playerAttack, Action<float> onOrangeAttackedAction, Action onYellowAttackedAction, Action onBlackAttackedAction, Action onSetBlackAction)
    {
        _playerAttack = playerAttack;
        OnOrangeAttacked = onOrangeAttackedAction;
        OnYellowAttacked = onYellowAttackedAction;
        OnBlackAttacked = onBlackAttackedAction;
        OnSetBlackColor = onSetBlackAction;
    }

    public void HasColor(Colors color, bool value)
    {
        _hasColorDict[color] = value;
        OnSetColor?.Invoke();
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
            Colors.Black => new Color32(80, 80, 80, 255),
            _ => Color.black,
        };
    }


    /*플레이어의 Color을 변경할 시 무조건 이 함수를 통해 변경해야 함.*/
    public void SetColorState(Colors _color)
    {
        SetPlayerColor(_color);
        GameManager.Instance.PlayerColor = _color;

        // 새로운 색 사용할 때 무기 그리도록 UI 띄우기
        var _hasNotBeenUsedColor = GameManager.Instance.CurrentWeaponSpriteList[(int)_color] == null;
        if (_hasNotBeenUsedColor && _color != Colors.Black)
        {
            StartDrawing(_color);
        }
        else
        {
            _playerAttack.CanAttack = true;
            ChangeColorStateByColors(_color);
            UseBasicWeapon(IsUsingBasicWeapon);

            ObjectPoolManager.Instance.SetColorName(_color);
            OnSetColor?.Invoke();
        }

    }


    // hasXXX 변수 설정 & 플레이어 무기 활성화
    void ChangeColorStateByColors(Colors _color)
    {
        switch (_color)
        {
            case Colors.Default:
                ChangeColorState(new DefaultColor(SetPlayerAnimatorBool));
                break;
            case Colors.Red:
                ChangeColorState(new RedColor(SetPlayerAnimatorBool));
                _playerAttack.RedWeapon.SetActive(true);
                break;
            case Colors.Yellow:
                _playerAttack.CanAttack = false;
                ChangeColorState(new YellowColor(OnYellowAttacked));
                _playerAttack.YellowAttackEffect.SetActive(true);
                break;
            case Colors.Blue:
                _playerAttack.BlueWeapon.SetActive(true);
                ChangeColorState(new BlueColor(SetPlayerAnimatorBool));
                break;
            case Colors.Green:
                _playerAttack.GreenWeapon.SetActive(true);
                ChangeColorState(new GreenColor(SetPlayerAnimatorBool));
                break;
            case Colors.Purple:
                ChangeColorState(new PurpleColor(SetPlayerAnimatorBool, ShakeCamera));
                _playerAttack.PurpleWeapon.SetActive(true);
                break;
            case Colors.Orange:
                ChangeColorState(new OrangeColor(OnOrangeAttacked));
                break;
            case Colors.Black:
                _playerAttack.BlackWeapon.SetActive(true);
                ChangeColorState(new BlackColor(OnBlackAttacked));
                OnSetBlackColor.Invoke();
                break;
        }
    }

    void ChangeColorState(IColorState _color)
    {
        SetPlayerColorState(_color);
        _playerAttack.RedWeapon.SetActive(false);
        _playerAttack.YellowAttackEffect.SetActive(false);
        _playerAttack.PurpleWeapon.SetActive(false);
    }


    public void StartDrawing(Colors _color)
    {
        // 게임 일시정지
        GameManager.Instance.PauseGame();

        UIManager.Instance.ShowPopupUI<UI_DrawCanvas>();
        //_hasBeenUsedColorSet.Add(_color);
            
        DrawManager.Instance.SetBrushColor(_color);
        DrawManager.Instance.OpenDrawing();
    }

    public void UseBasicWeapon(bool value)
    {
        IsUsingBasicWeapon = value;
        if (value)
            _playerAttack.SetBasicWeapon();
        //else
        //    _playerAttack.SetCustomWeapon();

        OnSaveColor?.Invoke();
    }

}