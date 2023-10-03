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
    enum Buttons
    {
        SettingBtn,
        Palette,
        Attack,
        Jump,
        Dash,
        Joystick,
    }


    private void Start()
    {
        Init();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        //    GetButton((int)Buttons.BackBtn).gameObject.BindEvent(OnBackBtnClicked);
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnSettingBtnClicked);
        GetButton((int)Buttons.Palette).gameObject.BindEvent(PaletteBtnClicked);
        GetButton((int)Buttons.Attack).gameObject.BindEvent(AttackBtnClicked);
        GetButton((int)Buttons.Jump).gameObject.BindEvent(JumpBtnClicked);
        GetButton((int)Buttons.Dash).gameObject.BindEvent(DashBtnClicked);
        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    public void OnSettingBtnClicked(PointerEventData data) // 설정 버튼 눌렀을 때
    {
        // 게임 일시정지 후 설정UI 띄우기

        GameManager.Instance.PauseGame();
        Managers.UI.ShowPopupUI<UI_Setting>();
    }

    public void PaletteBtnClicked(PointerEventData data) // 설정 버튼 눌렀을 때
    {
        // 게임 일시정지 후 설정UI 띄우기

        GameManager.Instance.PauseGame();
        Managers.UI.ShowPopupUI<UI_Palette>();
    }

    public void AttackBtnClicked(PointerEventData data)
    {
        player.GetComponent<PlayerController>().AttackDown();
    }

    public void JumpBtnClicked(PointerEventData data)
    {
        player.GetComponent<PlayerController>();
    }

    public void DashBtnClicked(PointerEventData data)
    {
        player.GetComponent<PlayerController>().AttackDown();
    }

}
