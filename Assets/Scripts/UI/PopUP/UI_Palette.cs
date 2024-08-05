using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Palette : UI_Popup
{

    public Colors Color;
    Image _canvasImg;
    Image _redrawImg;
    ColorManager _colormanager;

    //Colors redrawColor = Colors.def;
    Colors currentColor = Colors.Default;

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
        _colormanager = ColorManager.Instance;
        //redrawColor = GameManager.Instance.ReDrawItemColor;


        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        SettingPalette();
        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnBtnClicked);
        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(BackBtnClicked);
        GetButton((int)Buttons.ReDrawBtn).gameObject.BindEvent(ReDrawBtnClicked);

        _canvasImg = GetImage((int)Images.ColorImg).gameObject.GetComponent<Image>();
        _redrawImg = GetButton((int)Buttons.ReDrawBtn).gameObject.GetComponent<Image>();

        if (GameManager.Instance.PlayerColor == Colors.Default)
            GetButton((int)Buttons.ReDrawBtn).interactable = false;
        else
            _redrawImg.color = ColorManager.Instance.GetColor(GameManager.Instance.PlayerColor);
    }



    public void SettingPalette() 
    {
        GameObject Blue = GetButton((int)Buttons.BlueBtn).gameObject;
        GameObject Red = GetButton((int)Buttons.RedBtn).gameObject;
        GameObject Yellow = GetButton((int)Buttons.YellowBtn).gameObject;

        if (!_colormanager.HasBlue)
            Blue.SetActive(false);
        else
        {
            Blue.BindEvent(BlueBtnClicked);
            Blue.GetComponent<Image>().color = ColorManager.Instance.GetColor(Colors.Blue);
        }

        if (!_colormanager.HasRed)
            Red.SetActive(false);
        else
        {
            Red.BindEvent(RedBtnClicked);
            Red.GetComponent<Image>().color = ColorManager.Instance.GetColor(Colors.Red);
        }

        if (!_colormanager.HasYellow)
            Yellow.SetActive(false);
        else
        {
            Yellow.BindEvent(YellowBtnClicked);
            Yellow.GetComponent<Image>().color = ColorManager.Instance.GetColor(Colors.Yellow);
        }

        //GetButton((int)Buttons.ReDrawItem).gameObject.GetComponent<Image>().color = ColorManager.Instance.GetColor(redrawColor);

    }

    public void BackBtnClicked(PointerEventData data)
    {
        GameManager.Instance.ResumeGame();
    }

    public void OkayBtnClicked(PointerEventData data)
    {
        GameManager.Instance.ResumeGame();
        ColorManager.Instance.OnSetColor?.Invoke();
        if (currentColor != Colors.Default)
            _colormanager.SetColorState(currentColor);
    }

    public void ResetBtnBtnClicked(PointerEventData data)
    {
        currentColor = Colors.Default;
        ChangeColor(currentColor);
    }

    public void RedBtnClicked(PointerEventData data)
    {
        switch (currentColor)
        {
            case Colors.Default:
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
            case Colors.Default:
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
            case Colors.Default:
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
        _canvasImg.color = ColorManager.Instance.GetColor(color);
    }

    public void ReDrawBtnClicked(PointerEventData data)
    {
        GooglePlayManager.Instance.ShowAds(AdType.ReDraw);
        UIManager.Instance.ClosePopupUI();
        //ColorManager.Instance.StartDrawing(currentColor);
        //GameManager.Instance.ReDrawItemColor = Colors.def;
    }


}