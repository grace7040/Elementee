using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_LevelMenu : UI_Scene
{
    enum Buttons
    {
        Map1,
        Lobby,

    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Map1).gameObject.BindEvent(StartMapBtn);
        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobbyBtn);
        //  GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(SettingBtnClicked);

    }

    public void StartMapBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Map_0");
    }

    public void ToLobbyBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");

    }

}
