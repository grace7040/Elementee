using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Custom : UI_Popup
{

    enum Buttons
    {
        FaceBtn,
        SoundBtn,
        GoMainBtn,
        Lobby,

    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.FaceBtn).gameObject.BindEvent(StartFaceDrawing);
        GetButton((int)Buttons.SoundBtn).gameObject.BindEvent(SoundBtnClicked);
        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobbyBtn);

    }


    public void StartFaceDrawing(PointerEventData data)
    { 
        UIManager.Instance.ShowPopupUI<UI_DrawCanvas>();

        DrawManager.Instance.SetFaceColor();
        DrawManager.Instance.OpenDrawing();

        AudioManager.Instance.PlaySFX("UiClick");
    }


    public void SoundBtnClicked(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_SoundCustom>();
        AudioManager.Instance.PlaySFX("UiClick");
    }


    public void SceneJump(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
        AudioManager.Instance.PlaySFX("UiClick");
    }

    public void ToLobbyBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");
    }

}
