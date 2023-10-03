using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_InGame : UI_Popup
{
    GameObject player;
    GameObject hpBar;
    public int hpBarMAX;
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
    }


    private void Start()
    {
        Init();

        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterMove>().joystick = joystick;
    }

    private void FixedUpdate()
    {
        RectTransform barSize = hpBar.GetComponent<RectTransform>();
        print(GameManager.Instance.playerHP);
        float width = GameManager.Instance.HPBar() * hpBarMAX;
        barSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnSettingBtnClicked);
        GetButton((int)Buttons.Palette).gameObject.BindEvent(PaletteBtnClicked);
        GetButton((int)Buttons.Attack).gameObject.BindEvent(AttackBtnClicked);
        hpBar =  GetImage((int)Images.HP).gameObject;
        

        GameObject jump = GetButton((int)Buttons.Jump).gameObject;
        GameObject dash = GetButton((int)Buttons.Dash).gameObject;
    
        BindEvent(jump, JumpBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(jump, JumpBtnClickedUp, Define.UIEvent.UpClick);
        BindEvent(dash, DashBtnClickedDown, Define.UIEvent.DownClick);
        BindEvent(dash, DashBtnClickedUp, Define.UIEvent.UpClick);
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
    }


    // 플레이어 컨트롤

    public void AttackBtnClicked(PointerEventData data)
    {
        player.GetComponent<PlayerController>().AttackDown();
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

}
