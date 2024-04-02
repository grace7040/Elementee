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
    private Image canvasImg;
    private Image redrawImg;
    ColorManager C_Mgr;

    //Colors redrawColor = Colors.def;
    Colors currentColor = Colors.def;

    enum Buttons
    {
        OkayBtn,
        ResetBtn,
        HowtoBtn,
        BackBtn,
        RedBtn,
        YellowBtn,
        BlueBtn,
        ReDrawBtn,

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
        //redrawColor = GameManager.Instance.ReDrawItemColor;
        

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        SettingPalette();
        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnBtnClicked);
        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(BackBtnClicked);
        GetButton((int)Buttons.ReDrawBtn).gameObject.BindEvent(ReDrawBtnClicked);

        canvasImg = GetImage((int)Images.ColorImg).gameObject.GetComponent<Image>();
        redrawImg = GetButton((int)Buttons.ReDrawBtn).gameObject.GetComponent<Image>();

        if (GameManager.Instance.playerColor == Colors.def)
            GetButton((int)Buttons.ReDrawBtn).interactable = false;
        else
            redrawImg.color = ColorManager.Instance.GetColor(GameManager.Instance.playerColor);
    }



    public void SettingPalette()  // 가지고 있는 물감에 이벤트 바인딩 & 없는 물감은 끄기
    {
        // Blue 물감
        if (!C_Mgr.HasBlue)
            GetButton((int)Buttons.BlueBtn).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.BlueBtn).gameObject.BindEvent(BlueBtnClicked);

        // Red 물감
        if (!C_Mgr.HasRed)
            GetButton((int)Buttons.RedBtn).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.RedBtn).gameObject.BindEvent(RedBtnClicked);

        // Yellow 물감
        if (!C_Mgr.HasYellow)
            GetButton((int)Buttons.YellowBtn).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.YellowBtn).gameObject.BindEvent(YellowBtnClicked);

            //GetButton((int)Buttons.ReDrawItem).gameObject.GetComponent<Image>().color = ColorManager.Instance.GetColor(redrawColor);

    }

    public void BackBtnClicked(PointerEventData data)
    {
        //Managers.UI.ClosePopupUI();
        GameManager.Instance.ResumeGame();
    }

    public void OkayBtnClicked(PointerEventData data)
    {
        //Managers.UI.ClosePopupUI();
        GameManager.Instance.ResumeGame();
        ColorManager.Instance.OnSetColor?.Invoke();
        if (currentColor != Colors.def)
            C_Mgr.SetColorState(currentColor);  // 플레이어의 현재 Color에 적용
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


    // 물감 클릭했을 때 UI 색 변경
    public void ChangeColor(Colors color)
    {
        canvasImg.color = ColorManager.Instance.GetColor(color);
    }

    // 무기 다시 그리기
    public void ReDrawBtnClicked(PointerEventData data)
    {
        GetComponent<AdMob>().ShowAds();
        Managers.UI.ClosePopupUI();
        //ColorManager.Instance.StartDrawing(currentColor);
        //GameManager.Instance.ReDrawItemColor = Colors.def;
    }


}