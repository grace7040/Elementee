using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_GameOver : UI_Popup
{
    AdMob adMob;
    enum Buttons
    {
        ToMainBtn,
        RetryBtn,
        RevivalBtn,

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
        if (GameManager.Instance.IsFirstPlay)
        {
            GetButton((int)Buttons.RevivalBtn).gameObject.BindEvent(RevivalBtnClicked);
            GetButton((int)Buttons.RevivalBtn).interactable = true;
        }
        adMob = GetComponent<AdMob>();
    }

    public void RevivalBtnClicked(PointerEventData data)
    {
        adMob.ShowAds();
        UIManager.Instance.ClosePopupUI();
        // 광고 + 부활하는거 여기에~
    }

    public void OnResumeBtnClicked(PointerEventData data)
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRetryBtnClicked(PointerEventData data)
    {
        GameManager.Instance.RetryGame();
        //GameManager.Instance.Revival();
    }

    public void ToMainBtnClicked(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
    }
}