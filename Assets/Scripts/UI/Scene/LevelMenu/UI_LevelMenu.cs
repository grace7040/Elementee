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
    public List<GameObject> mapBtns = new List<GameObject>();

    enum Buttons
    {
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
        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobbyBtn);

    }


    public void ToLobbyBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");
    }

}
