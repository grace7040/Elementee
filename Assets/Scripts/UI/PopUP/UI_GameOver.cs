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
        //Bind<TMP_Text>(typeof(Texts)); 
        //Bind<GameObject>(typeof(GameObjects)); 
        //Bind<Image>(typeof(Images)); 

        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(ToMainBtnClicked);
        //GetButton((int)Buttons.ResumeBtn).gameObject.BindEvent(OnResumeBtnClicked);
        GetButton((int)Buttons.RetryBtn).gameObject.BindEvent(OnRetryBtnClicked);

        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
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

    public void ToMainBtnClicked(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
    }
}