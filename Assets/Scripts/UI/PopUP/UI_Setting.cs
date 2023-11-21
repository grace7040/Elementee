using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Setting  : UI_Popup
{
    enum Buttons
    {
        ToMainBtn,
        RetryBtn,
        ResumeBtn,

    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons)); 


        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(ToMaineBtnClicked);
        GetButton((int)Buttons.ResumeBtn).gameObject.BindEvent(OnResumeBtnClicked);
        GetButton((int)Buttons.RetryBtn).gameObject.BindEvent(OnRetryBtnClicked);
    }

    public void ToMaineBtnClicked(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
        Managers.UI.ClosePopupUI();
    }

    public void OnResumeBtnClicked(PointerEventData data)
    {
        GameManager.Instance.ResumeGame();
        Managers.UI.ClosePopupUI();
    }    
    
    public void OnRetryBtnClicked(PointerEventData data)
    {
        GameManager.Instance.RetryGame();
        Managers.UI.ClosePopupUI();
    }
}