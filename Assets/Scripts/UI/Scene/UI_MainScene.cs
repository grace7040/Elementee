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
        StoreBtn,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartBtn).gameObject.BindEvent(StartBtnClicked);
        GetButton((int)Buttons.CustomBtn).gameObject.BindEvent(CustomBtnClicked);
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(StoreBtnClicked);

    }

    public void StartBtnClicked(PointerEventData data)
    {
        SceneManager.LoadScene("LevelMenu");
        AudioManager.Instacne.PlaySFX("UiClick");
    }

    public void SettingBtnClicked(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_MainSetting>();

    }
    public void CustomBtnClicked(PointerEventData data)
    {
        SceneManager.LoadScene("Custom");
        Time.timeScale = 1;

        AudioManager.Instacne.PlaySFX("UiClick");
    }

    public void StoreBtnClicked(PointerEventData data)
    {
        SceneManager.LoadScene("Store");
        Time.timeScale = 1;

        AudioManager.Instacne.PlaySFX("UiClick");
    }
}
