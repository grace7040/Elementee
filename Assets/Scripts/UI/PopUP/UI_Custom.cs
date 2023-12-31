﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Custom : UI_Popup
{
    public GameObject Child_UI;
    enum Buttons
    {
        FaceBtn,
        SoundBtn,
        GoMainBtn,

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

        GetButton((int)Buttons.FaceBtn).gameObject.BindEvent(StartFaceDrawing);
        GetButton((int)Buttons.GoMainBtn).gameObject.BindEvent(SceneJump);
        GetButton((int)Buttons.SoundBtn).gameObject.BindEvent(SoundBtnClicked);

    }

    public void StartFaceDrawing(PointerEventData data)
    {
       // ColorManager.Instance.StartDrawing(Colors.black);
        
        Managers.UI.ShowPopupUI<UI_DrawCanvas>();

        DrawManager.Instance.SetFaceColor();
        DrawManager.Instance.OpenDrawing();

        Child_UI.SetActive(false);

    }

    public void SoundBtnClicked(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_SoundCustom>();
    }


        public void SceneJump(PointerEventData data)
    {
        //ClosePopupUI();
        //SceneManager.LoadScene(2);
        GameManager.Instance.GoToMainMenu();

    }
}
