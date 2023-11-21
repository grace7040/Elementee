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
    public SoundObjects currentObject;


    AudioClip record;
    AudioSource aud;

    

    public enum SoundObjects
    {
        def,
        Red,
        Yellow,
        Blue,
    }



    enum Buttons
    {
        Red,
        Yellow,
        Blue,
        Save,
        Record,
        Play,
        Exit,

    }

    enum Images
    {
        Image,
    }


    private void Start()
    {
        Init();
        aud = GetComponent<AudioSource>();
    }


    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        // 사운드 오브젝트들
        GetButton((int)Buttons.Red).gameObject.BindEvent(Red);
        GetButton((int)Buttons.Yellow).gameObject.BindEvent(Yellow);
        GetButton((int)Buttons.Blue).gameObject.BindEvent(Blue);


        GetButton((int)Buttons.Save).gameObject.BindEvent(SaveClip);
        GetButton((int)Buttons.Record).gameObject.BindEvent(RecordBtnClicked);
        GetButton((int)Buttons.Play).gameObject.BindEvent(PlayBtnClicked);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(ExitBtnClicked);
    }


    // 현재 상태 & 미리보기 이미지 업데이트
    public void SetSoundObject(SoundObjects obj)
    {
        currentObject = obj;
        GetImage((int)Images.Image).sprite = sprites[(int)currentObject];
    }


    public void RecordBtnClicked(PointerEventData data)
    {
      
        if (currentObject != SoundObjects.def)
        {
            record = Microphone.Start(Microphone.devices[0].ToString(), false, 1, 44100);

            
            // AudioSource에 할당
            aud.clip = record;
        }
    }

    public void PlayBtnClicked(PointerEventData data)
    {
        if (currentObject != SoundObjects.def)
        {
            aud.Play();
        }
    }

    public void ExitBtnClicked(PointerEventData data)
    {
        SetSoundObject(SoundObjects.def);
    }

    public void SaveClip(PointerEventData data)
    {
        string name = Enum.GetName(typeof(SoundObjects), currentObject);

        if(currentObject != SoundObjects.def)
        {
            SavWav.Save("C:/Users/user/wkspaces/Elementee/Assets/Cherry/Records/" + name, aud.clip);
            //AudioManager에 저장

            AudioClip save = Resources.Load("C:/Users/user/wkspaces/Elementee/Assets/Cherry/Records/" + name, typeof(AudioClip)) as AudioClip;
            //AudioManager.Instacne.SetSFX(name, save);
        }

    }


    public void Red(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Red);
    }

    public void Yellow(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Yellow);
    }

    public void Blue(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Blue);
    }






}
