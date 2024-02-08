using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_GameOver : UI_Popup
{
    enum Buttons
    {
        ToMainBtn,
        RetryBtn,
        //ResumeBtn,

    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); 

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(ToMainBtnClicked);
        GetButton((int)Buttons.RetryBtn).gameObject.BindEvent(OnRetryBtnClicked);

        
    }

    public void OnResumeBtnClicked(PointerEventData data)
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRetryBtnClicked(PointerEventData data)
    {
        GameManager.Instance.RetryGame();
    }

    public void ToMainBtnClicked(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
    }
}