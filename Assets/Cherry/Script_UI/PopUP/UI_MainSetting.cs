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

        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(OnBackBtnClicked);
        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(OnBackBtnClicked);
        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    public void OnBackBtnClicked(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
        Debug.Log("끄자");
    }
}