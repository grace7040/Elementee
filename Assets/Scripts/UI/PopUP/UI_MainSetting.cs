using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_MainSetting  : UI_Popup
{
    enum Buttons
    {
        SoundBtn,
        TutoBtn,
        ToMainBtn,
        BackBtn,
    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons)); 

        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(OnBackBtnClicked);
        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(OnBackBtnClicked);
    }

    public void OnBackBtnClicked(PointerEventData data)
    {
        UIManager.Instance.ClosePopupUI();
    }
}