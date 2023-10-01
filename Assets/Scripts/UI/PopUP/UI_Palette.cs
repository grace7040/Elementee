using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Palette : UI_Popup
{
    public enum Colors { White, Red, Yellow, Blue, Orange, Green, Purple, Black };
    public GameObject canvas;

    Colors currentColor = Colors.White;

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
        currentColor = Colors.White;
        ChangeColor(currentColor);
    }

    public void RedBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.White:
                currentColor = Colors.Red;
                break;
            case Colors.Yellow:
                currentColor = Colors.Orange;
                break;
            case Colors.Blue:
                currentColor = Colors.Purple;
                break;
            case Colors.Green:
                currentColor = Colors.Black;
                break;
        }

        ChangeColor(currentColor);
    }

    public void YellowBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.White:
                currentColor = Colors.Yellow;
                break;
            case Colors.Red:
                currentColor = Colors.Orange;
                break;
            case Colors.Blue:
                currentColor = Colors.Green;
                break;            
            case Colors.Purple:
                currentColor = Colors.Black;
                break;
        }

        ChangeColor(currentColor);

    }

    public void BlueBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.White:
                currentColor = Colors.Blue;
                break;
            case Colors.Yellow:
                currentColor = Colors.Green;
                break;
            case Colors.Red:
                currentColor = Colors.Purple;
                break;
            case Colors.Orange:
                currentColor = Colors.Black;
                break;
        }

        ChangeColor(currentColor);
    }


    public void ChangeColor(Colors color)
    {
        Image canvasColor = canvas.GetComponent<Image>();

        switch (color)
        {
            case Colors.White:
                canvasColor.color = Color.white;
                break;
            case Colors.Red:
                canvasColor.color = Color.red;
                break;
            case Colors.Yellow:
                canvasColor.color = Color.yellow;
                break;
            case Colors.Blue:
                canvasColor.color = Color.blue;
                break;
            case Colors.Orange:
                canvasColor.color = new Color32(255, 158, 66, 255);
                break; 
            case Colors.Green:
                canvasColor.color = Color.green;
                break; 
            case Colors.Purple:
                canvasColor.color = new Color32(150, 54, 183, 255);
                break;
            case Colors.Black:
                canvasColor.color = Color.black;
                break;
        }
    }


}