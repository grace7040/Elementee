using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_MainScene : UI_Scene
{
    enum Buttons
    {
        StartBtn,
        SettingBtn,

    }

    //enum Texts
    //{
    //    PointText,
    //    ScoreText
    //}

    //enum GameObjects
    //{
    //    TestObject,
    //}

    //enum Images
    //{
    //    ItemIcon,
    //}

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartBtn).gameObject.BindEvent(StartBtnClicked);
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(SettingBtnClicked);

    }

    public void StartBtnClicked(PointerEventData data)
    {
        Debug.Log("게임 시작!");
    }

    public void SettingBtnClicked(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_MainSetting>();
    }
}
