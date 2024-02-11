using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_MainScene : UI_Scene
{
    enum Buttons
    {
        StartBtn,
        CustomBtn,

    }

    private void Start()
    {
        Init();
        DataManager.Instance.JsonLoad();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartBtn).gameObject.BindEvent(StartBtnClicked);
      //  GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(SettingBtnClicked);
        GetButton((int)Buttons.CustomBtn).gameObject.BindEvent(CustomBtnClicked);

    }

    public void StartBtnClicked(PointerEventData data)
    {
        SceneManager.LoadScene("LevelMenu");
        AudioManager.Instacne.PlaySFX("UiClick");
    }

    public void SettingBtnClicked(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_MainSetting>();

    }

    public void CustomBtnClicked(PointerEventData data)
    {
        SceneManager.LoadScene("Custom");
        Time.timeScale = 1;

        AudioManager.Instacne.PlaySFX("UiClick");
    }
}
