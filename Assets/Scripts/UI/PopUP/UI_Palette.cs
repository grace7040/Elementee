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
    ColorManager C_Mgr;

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
        C_Mgr = ColorManager.Instance;

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        SettingPalette();
        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnBtnClicked);
        
        canvas = GetImage((int)Images.ColorImg).gameObject;
    }

    public void SettingPalette()  // 가지고 있는 물감만 이벤트 바인딩
    {
        if (C_Mgr.hasBlue)
            GetButton((int)Buttons.BlueBtn).gameObject.BindEvent(BlueBtnClicked);
        if (C_Mgr.hasRed)
            GetButton((int)Buttons.RedBtn).gameObject.BindEvent(RedBtnClicked);
        if (C_Mgr.hasYellow)
            GetButton((int)Buttons.YellowBtn).gameObject.BindEvent(YellowBtnClicked);
    }


    public void OkayBtnClicked(PointerEventData data)
    {
        // 플레이어의 State 바꾸는 부분 추가
        // 플레이어가 소유한 물감 수정

        switch (currentColor)
        {
            case Colors.def:
                break;
            case Colors.red:
                C_Mgr.hasRed = false;
                C_Mgr.SetColorState(new RedColor());
                break;
            case Colors.yellow:
                C_Mgr.hasYellow = false;
                C_Mgr.SetColorState(new YellowColor());
                break;
            case Colors.blue:
                C_Mgr.hasBlue = false;
                C_Mgr.SetColorState(new BlueColor());
                break;
            case Colors.green:
                C_Mgr.hasYellow = false;
                C_Mgr.hasBlue = false;
                C_Mgr.SetColorState(new GreenColor());
                break;
            case Colors.purple:
                C_Mgr.hasRed = false;
                C_Mgr.hasBlue = false;
                C_Mgr.SetColorState(new PurpleColor());
                break;
            case Colors.orange:
                C_Mgr.hasYellow = false;
                C_Mgr.hasRed = false;
                C_Mgr.SetColorState(new OrangeColor());
                break;
            case Colors.black:
                C_Mgr.hasYellow = false;
                C_Mgr.hasRed = false;
                C_Mgr.hasBlue = false;
                C_Mgr.SetColorState(new BlackColor());
                break;
        }
       
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
                canvasColor.color = new Color32(254, 120, 120, 255);
                break;
            case Colors.yellow:
                canvasColor.color = new Color32(255, 229, 73, 255);
                break;
            case Colors.blue:
                canvasColor.color = new Color32(133, 151, 255, 255);
                break;
            case Colors.orange:
                canvasColor.color = new Color32(255, 175, 61, 255);
                break; 
            case Colors.green:
                canvasColor.color = new Color32(43, 202, 99, 255);
                break; 
            case Colors.purple:
                canvasColor.color = new Color32(167, 54, 200, 255);
                break;
            case Colors.black:
                canvasColor.color = Color.black;
                break;
        }
    }


}