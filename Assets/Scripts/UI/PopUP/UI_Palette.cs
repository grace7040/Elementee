using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Palette : UI_Popup
{

    public Colors color;
    public GameObject canvas;

    Colors currentColor = Colors.def;

    enum Buttons
    {
        OkayBtn,
        ResetBtn,
        HowtoBtn,
        RedBtn,
        YellowBtn,
        BlueBtn,

    }


    enum Images
    {
        ColorImg,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images)); 


        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.RedBtn).gameObject.BindEvent(RedBtnClicked);
        GetButton((int)Buttons.YellowBtn).gameObject.BindEvent(YellowBtnClicked);
        GetButton((int)Buttons.BlueBtn).gameObject.BindEvent(BlueBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnBtnClicked);
        
        canvas = GetImage((int)Images.ColorImg).gameObject;
    }

    public void SettingPalette()  // 가지고 있는 물감만 이벤트 바인딩
    {
        // 플레이어가 어떤 물감을 가지고 있는 지 알아야함
    }


    public void OkayBtnClicked(PointerEventData data)
    {
        // 플레이어의 State 바꾸는 부분 추가
        // 플레이어가 소유한 물감 수정

        GameManager.Instance.ResumeGame();
        Managers.UI.ClosePopupUI();
    }

    public void ResetBtnBtnClicked(PointerEventData data)
    {
        currentColor = Colors.def;
        ChangeColor(currentColor);
    }

    public void RedBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.def:
                currentColor = Colors.red;
                break;
            case Colors.yellow:
                currentColor = Colors.orange;
                break;
            case Colors.blue:
                currentColor = Colors.purple;
                break;
            case Colors.green:
                currentColor = Colors.black;
                break;
        }

        ChangeColor(currentColor);
    }

    public void YellowBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.def:
                currentColor = Colors.yellow;
                break;
            case Colors.red:
                currentColor = Colors.orange;
                break;
            case Colors.blue:
                currentColor = Colors.green;
                break;            
            case Colors.purple:
                currentColor = Colors.black;
                break;
        }

        ChangeColor(currentColor);

    }

    public void BlueBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.def:
                currentColor = Colors.blue;
                break;
            case Colors.yellow:
                currentColor = Colors.green;
                break;
            case Colors.red:
                currentColor = Colors.purple;
                break;
            case Colors.orange:
                currentColor = Colors.black;
                break;
        }

        ChangeColor(currentColor);
    }


    public void ChangeColor(Colors color)
    {
        Image canvasColor = canvas.GetComponent<Image>();

        switch (color)
        {
            case Colors.def:
                canvasColor.color = Color.white;
                break;
            case Colors.red:
                canvasColor.color = Color.red;
                break;
            case Colors.yellow:
                canvasColor.color = Color.yellow;
                break;
            case Colors.blue:
                canvasColor.color = Color.blue;
                break;
            case Colors.orange:
                canvasColor.color = new Color32(255, 158, 66, 255);
                break; 
            case Colors.green:
                canvasColor.color = Color.green;
                break; 
            case Colors.purple:
                canvasColor.color = new Color32(150, 54, 183, 255);
                break;
            case Colors.black:
                canvasColor.color = Color.black;
                break;
        }
    }


}