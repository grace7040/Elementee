using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Game : UI_Scene
{
    public GameObject player;
    public TextMeshProUGUI Score;
    Colors player_color = Colors.def;


    GameObject jump;
    GameObject dash;
    GameObject attack;
    Image hpBar;

    // 팔레트 물감
    Image Red_IMG;
    Image Yellow_IMG;
    Image Blue_IMG;

    public float hpBarMAX;
    public VariableJoystick joystick;

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
    }

    enum Texts
    {
        StarCount,
    }

    public void Start()
    {
        Init();
        ObjectManager.Instance.UI_InGame_Ready = true;
    }

    private void FixedUpdate()
    {
        // 플레이어 체력바
        RectTransform barSize = hpBar.GetComponent<RectTransform>();
        float width = GameManager.Instance.HPBar() * hpBarMAX;
        barSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        int star = GameManager.Instance.starCount;
        Score.text = star + "/ 10";
        //GetText((int)Texts.StarCount).text = 
    }

    public void SetPalette()
    {
        Blue_IMG.gameObject.SetActive(ColorManager.Instance.HasBlue);
        Red_IMG.gameObject.SetActive(ColorManager.Instance.HasRed);
        Yellow_IMG.gameObject.SetActive(ColorManager.Instance.HasYellow);

        Colors current_color32 = GameManager.Instance.playerColor;

        // 플레이어 state에 따른 색 변경
        if (player_color != current_color32)
        {
            player_color = current_color32;

            Color32 color32 = ColorManager.Instance.GetColor(player_color);
            Color32 alpha_color32_1 = new Color32(color32.r, color32.g, color32.b, 150);
            Color32 alpha_color32_2 = new Color32(color32.r, color32.g, color32.b, 200);

            hpBar.color = alpha_color32_2;
            GetButton((int)Buttons.Attack).gameObject.GetComponent<Image>().color = alpha_color32_2;
            jump.GetComponent<Image>().color = alpha_color32_1;
            dash.GetComponent<Image>().color = alpha_color32_1;

        }
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
        hpBar = GetImage((int)Images.HP);


        jump = GetButton((int)Buttons.Jump).gameObject;
        dash = GetButton((int)Buttons.Dash).gameObject;
        attack = GetButton((int)Buttons.Attack).gameObject;

        BindEvent(jump, JumpBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(jump, JumpBtnClickedUp, Define.UIEvent.UpClick);
        BindEvent(dash, DashBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(dash, DashBtnClickedUp, Define.UIEvent.UpClick);
        BindEvent(attack, AttackBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(attack, AttackBtnClickedUp, Define.UIEvent.UpClick);


        // 물감 오브젝트 받아두기 + ColorManger에 저장된 색으로 변경
        Red_IMG = GetImage((int)Images.Red);
        Red_IMG.color = ColorManager.Instance.GetColor(Colors.red);

        Yellow_IMG = GetImage((int)Images.Yellow);
        Yellow_IMG.color = ColorManager.Instance.GetColor(Colors.yellow);

        Blue_IMG = GetImage((int)Images.Blue);
        Blue_IMG.color = ColorManager.Instance.GetColor(Colors.blue);


        // Palette 세팅
        SetPalette();
        ColorManager.Instance.OnSetColor += SetPalette;

        GameManager.Instance.SetJoystick = () => {
            FindObjectOfType<CharacterMove>().joystick = joystick;
        };


        // hpBar 길이 받아두기
        hpBarMAX = hpBar.gameObject.GetComponent<RectTransform>().rect.width;
    }

    public void OnSettingBtnClicked(PointerEventData data) // 설정 버튼 눌렀을 때
    {
        // 게임 일시정지 후 설정UI 띄우기

        GameManager.Instance.PauseGame();
        Managers.UI.ShowPopupUI<UI_Setting>();
    }

    public void PaletteBtnClicked(PointerEventData data)
    {
        // 게임 일시정지 후 설정UI 띄우기

        GameManager.Instance.PauseGame();
        Managers.UI.ShowPopupUI<UI_Palette>();
        //  Managers.UI.ShowPopupUI<UI_Palette>();

    }


    // 플레이어 컨트롤

    public void AttackBtnClickedDown(PointerEventData data)
    {
        player.GetComponent<PlayerController>().AttackDown();
    }

    public void AttackBtnClickedUp(PointerEventData data)
    {
        player.GetComponent<PlayerController>().AttackUp();
    }

    public void JumpBtnClickedDown(PointerEventData data)
    {
        player.GetComponent<CharacterMove>().JumpDown();
    }

    public void JumpBtnClickedUp(PointerEventData data)
    {
        player.GetComponent<CharacterMove>().JumpUp();
    }

    public void DashBtnClickedDown(PointerEventData data)
    {
        player.GetComponent<CharacterMove>().DashDown();
    }

    public void DashBtnClickedUp(PointerEventData data)
    {
        player.GetComponent<CharacterMove>().DashUp();
    }

    private void OnDestroy()
    {
        if (ColorManager.Instance != null)
            ColorManager.Instance.OnSetColor -= SetPalette;
    }

}
