using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons)); // 버튼 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        Bind<TMP_Text>(typeof(Texts));  // 텍스트 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        Bind<GameObject>(typeof(GameObjects));  // 빈 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        Bind<Image>(typeof(Images));  // 이미지 오브젝트들 가져와 dictionary인 _objects에 바인딩. 

        // (확장 메서드) 버튼(go)에 📜UI_EventHandler를 붙이고 액션에 OnButtonClicked 함수를 OnClickHandler (디폴트)등록한다.
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        // 이미지(go)에 📜UI_EventHandler를 붙이고 파라미터로 넘긴 람다 함수를 OnDragHandler 액션에 등록한다.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        Get<TMP_Text>((int)Texts.ScoreText).text = $"점수 : {_score}";
    }
}