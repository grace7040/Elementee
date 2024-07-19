using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Game : UI_Scene
{

    public TextMeshProUGUI Coin;
    public List<Image> Stars = new List<Image>();
    Colors player_color = Colors.Default;

    // Button
    GameObject _jump;
    GameObject _dash;
    GameObject _attack;

    // Image
    Image _hpBar;
    Image _Red_IMG;
    Image _Yellow_IMG;
    Image _Blue_IMG;
    Image _Attack_Cool_Time_IMG;
    Image _PotalArr_IMG;

    public float HpBarMAX;
    public VariableJoystick Joystick;

    //Player
    float _attack_cool_time = 0f;
    float _attack_cool_count = 0f;
    bool _canAttack;
    int _starCount = 0;
    PlayerController _playerController;
    PlayerAttack _playerAttack;
    CharacterMove _characterMove;
    GameObject _player;

    //Potal
    float _angle;
    Vector2 _potalVec;
    Vector2 _playerVec;

    enum Buttons
    {
        SettingBtn,
        Palette,
        Attack,
        Jump,
        Dash,
        Joystick,
    }

    enum Images
    {
        HP,
        Red,
        Yellow,
        Blue,
        Attack_Cool_Time,
        PotalArrow,
    }

    enum Texts
    {
        StarCount,
        CoinCount,
    }

    public void Start()
    {
        Init();

        _player = GameManager.Instance.Player;
        _playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        _playerAttack = GameManager.Instance.Player.GetComponent<PlayerAttack>();
        _characterMove = GameManager.Instance.Player.GetComponent<CharacterMove>();

        GameManager.Instance.UIGame = this;

    }


    public void UpdateHPBar(int current, int max)
    {
        _hpBar.fillAmount = (float)current / max;
    }

    public void UpdateCoinUI()
    {
        Coin.text = GameManager.Instance.MapCoin.ToString();
    }

    public void UpdateStarUI()
    {
        if (_starCount != GameManager.Instance.CurrentStar)
        {
            _starCount = GameManager.Instance.CurrentStar;
            for (int i = 0; i < _starCount; i++)
            {
                Stars[i].GetComponent<Image>().color = Color.white;
            }
        }

    }

    public void SetPalette()
    {
        _Blue_IMG.gameObject.SetActive(ColorManager.Instance.HasBlue);
        _Red_IMG.gameObject.SetActive(ColorManager.Instance.HasRed);
        _Yellow_IMG.gameObject.SetActive(ColorManager.Instance.HasYellow);

        Colors current_color32 = GameManager.Instance.PlayerColor;

        // 플레이어 state에 따른 색 변경
        if (player_color != current_color32)
        {
            player_color = current_color32;

            Color32 color32 = ColorManager.Instance.GetColor(player_color);
            Color32 alpha_color32_1 = new Color32(color32.r, color32.g, color32.b, 150);
            Color32 alpha_color32_2 = new Color32(color32.r, color32.g, color32.b, 200);

            _hpBar.color = alpha_color32_2;
            GetButton((int)Buttons.Attack).gameObject.GetComponent<Image>().color = alpha_color32_2;
            _jump.GetComponent<Image>().color = alpha_color32_1;
            _dash.GetComponent<Image>().color = alpha_color32_1;

        }
        if (player_color == Colors.Yellow)
            GetImage((int)Images.Attack_Cool_Time).gameObject.SetActive(true);
        else
            GetImage((int)Images.Attack_Cool_Time).gameObject.SetActive(false);

    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnSettingBtnClicked);
        GetButton((int)Buttons.Palette).gameObject.BindEvent(PaletteBtnClicked);
        //  GetButton((int)Buttons.Attack).gameObject.BindEvent(AttackBtnClicked);
        _hpBar = GetImage((int)Images.HP);


        _jump = GetButton((int)Buttons.Jump).gameObject;
        _dash = GetButton((int)Buttons.Dash).gameObject;
        _attack = GetButton((int)Buttons.Attack).gameObject;

        BindEvent(_jump, JumpBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(_jump, JumpBtnClickedUp, Define.UIEvent.UpClick);
        BindEvent(_dash, DashBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(_dash, DashBtnClickedUp, Define.UIEvent.UpClick);
        BindEvent(_attack, AttackBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(_attack, AttackBtnClickedUp, Define.UIEvent.UpClick);


        // 물감 오브젝트 받아두기 + ColorManger에 저장된 색으로 변경
        _Red_IMG = GetImage((int)Images.Red);
        _Red_IMG.color = ColorManager.Instance.GetColor(Colors.Red);

        _Yellow_IMG = GetImage((int)Images.Yellow);
        _Yellow_IMG.color = ColorManager.Instance.GetColor(Colors.Yellow);


        _Blue_IMG = GetImage((int)Images.Blue);
        _Blue_IMG.color = ColorManager.Instance.GetColor(Colors.Blue);

        _Attack_Cool_Time_IMG = GetImage((int)Images.Attack_Cool_Time);


        // Palette 세팅
        SetPalette();
        ColorManager.Instance.OnSetColor += SetPalette;

        //GameManager.Instance.SetJoystick = () => {
        //    FindObjectOfType<CharacterMove>().joystick = joystick;
        //};


        // HpBar
        HpBarMAX = _hpBar.gameObject.GetComponent<RectTransform>().rect.width;

        // Potal
        _PotalArr_IMG = GetImage((int)Images.PotalArrow);
        if (GameManager.Instance.CurrentPotal != null)
            _potalVec = GameManager.Instance.CurrentPotal.transform.position;


        GetImage((int)Images.Attack_Cool_Time).gameObject.SetActive(false);

    }

    public void OnSettingBtnClicked(PointerEventData data)
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.ShowPopupUI<UI_Setting>();
    }

    public void PaletteBtnClicked(PointerEventData data)
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.ShowPopupUI<UI_Palette>();

    }


    public void AttackBtnClickedDown(PointerEventData data)
    {
        if (_playerAttack.canAttack)
        {
            _playerAttack.AttackDown();
            _attack_cool_time = _playerController.AttackCoolTime;
            _attack_cool_count = _attack_cool_time;

            GetImage((int)Images.Attack_Cool_Time).gameObject.SetActive(true);
            _canAttack = true;
        }
    }

    public void AttackBtnClickedUp(PointerEventData data)
    {
        _playerAttack.AttackUp();
    }

    public void JumpBtnClickedDown(PointerEventData data)
    {
        _characterMove.JumpDown();
    }

    public void JumpBtnClickedUp(PointerEventData data)
    {
        _characterMove.JumpUp();
    }

    public void DashBtnClickedDown(PointerEventData data)
    {
        _characterMove.DashDown();
    }

    public void DashBtnClickedUp(PointerEventData data)
    {
        _characterMove.DashUp();
    }

    private void OnDestroy()
    {
        if (ColorManager.Instance != null)
            ColorManager.Instance.OnSetColor -= SetPalette;
    }

    private void Update()
    {
        if (_canAttack)
        {
            StartCoroutine(nameof(SkillTimeChk));
        }

        // Potal Arr
        _playerVec = _player.transform.position;
        _angle = Mathf.Atan2(_potalVec.y - _playerVec.y, _potalVec.x - _playerVec.x) * Mathf.Rad2Deg;
        _PotalArr_IMG.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    IEnumerator SkillTimeChk()
    {
        yield return null;
        if (_attack_cool_count > 0)
        {
            _attack_cool_count -= Time.deltaTime;

            if (_attack_cool_count < 0)
            {

                _attack_cool_count = 0;
                _canAttack = false;
                GetImage((int)Images.Attack_Cool_Time).gameObject.SetActive(false);

                _attack_cool_count = _playerController.AttackCoolTime;

            }
            float time = _attack_cool_count / _playerController.AttackCoolTime;
            GetImage((int)Images.Attack_Cool_Time).fillAmount = time;

        }

    }


}