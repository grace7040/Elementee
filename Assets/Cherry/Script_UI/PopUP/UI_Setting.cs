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

        Bind<Button>(typeof(Buttons)); // 버튼 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        //Bind<TMP_Text>(typeof(Texts));  // 텍스트 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        //Bind<GameObject>(typeof(GameObjects));  // 빈 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        //Bind<Image>(typeof(Images));  // 이미지 오브젝트들 가져와 dictionary인 _objects에 바인딩. 


        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(OnBackBtnClicked);

        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    public void OnBackBtnClicked(PointerEventData data)
    {
        Debug.Log("끄자");
    }
}