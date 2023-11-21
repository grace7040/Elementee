using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_SoundCustom : UI_Popup
{

    public Sprite[] sprites;


    AudioClip record;
    AudioSource aud;



    enum Buttons
    {
        Red,
        Yellow,
        Blue,
        Save,

    }


    private void Start()
    {
        Init();
    }


    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Red).gameObject.BindEvent(Recording);
        GetButton((int)Buttons.Save).gameObject.BindEvent(SaveClip);
        //  GetButton((int)Buttons.SceneBtn).gameObject.BindEvent(SceneJump);
        //  GetButton((int)Buttons.SoundBtn).gameObject.BindEvent(SettingBtnClicked);

    }

    public void Recording(PointerEventData data)
    {
        record = Microphone.Start(Microphone.devices[0].ToString(), false, 1, 44100);
        aud.clip = record;
    }

    public void SaveClip(PointerEventData data)
    {
        SavWav.Save("C:/Users/user/wkspaces/Elementee/Assets/Cherry/Records/Test" + this.gameObject.name, aud.clip);
    }

    //public void SceneJump(PointerEventData data)
    //{
    //    //ClosePopupUI();
    //    //SceneManager.LoadScene(2);
    //    GameManager.Instance.RetryGame();

    //}
}
