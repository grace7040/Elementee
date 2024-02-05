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
    public int recordTime = 1;
    public Sprite[] sprites;
    public SoundObjects currentObject;
    public AudioClip test;
    public GameObject blockImage;

    public GameObject recordAnim;

    AudioClip record;
    AudioSource aud;

    

    public enum SoundObjects
    {
        def,
        Red,
        Yellow,
        Blue,
        Orange,
        Green,
        Purple,
        Black,
        Jump,
        Dash,
        Dead,
        Hurt,
    }


    enum Buttons
    {
        // 사운드 오브젝트들
        Red,
        Yellow,
        Blue,
        Orange,
        Green,
        Purple,
        Black,
        Jump,
        Dash,
        Dead,
        Hurt,

        // 나머지
        Save,
        Record,
        Pause,
        Play,
        Exit,
        Back,
        Default,

    }

    enum Images
    {
        Image,
        RecordAnim,
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
        GetButton((int)Buttons.Green).gameObject.BindEvent(Green);
        GetButton((int)Buttons.Orange).gameObject.BindEvent(Orange);
        GetButton((int)Buttons.Purple).gameObject.BindEvent(Purple);
        GetButton((int)Buttons.Jump).gameObject.BindEvent(Jump);
        GetButton((int)Buttons.Dash).gameObject.BindEvent(Dash);
        GetButton((int)Buttons.Dead).gameObject.BindEvent(Dead);
        GetButton((int)Buttons.Hurt).gameObject.BindEvent(Hurt);


        GetButton((int)Buttons.Save).gameObject.BindEvent(SaveClip);
        GetButton((int)Buttons.Record).gameObject.BindEvent(RecordBtnClicked);
        GetButton((int)Buttons.Play).gameObject.BindEvent(PlayBtnClicked);
        GetButton((int)Buttons.Default).gameObject.BindEvent(DafaultBtnClicked);
        GetButton((int)Buttons.Back).gameObject.BindEvent(BackBtnClicked);
        GetButton((int)Buttons.Pause).gameObject.BindEvent(PauseBtnClicked);
    }


    // 현재 상태 & 미리보기 이미지 업데이트
    public void SetSoundObject(SoundObjects obj)
    {
        currentObject = obj;
        GetImage((int)Images.Image).sprite = sprites[(int)currentObject];
    }

    public void PauseBtnClicked(PointerEventData data)
    {
        aud.Pause();
    }

    public void RecordBtnClicked(PointerEventData data)
    {
      
        if (currentObject != SoundObjects.def)
        {
            //  recordAnim.Play();
            recordAnim.GetComponent<Animator>().Play("Record", -1, 0f);

            record = Microphone.Start(Microphone.devices[0].ToString(), false, recordTime, 44100);
            aud.clip = record;
            blockImage.SetActive(true);
            Invoke("OffBlockImg", recordTime);
        }


    }

    public void DafaultBtnClicked(PointerEventData data)
    {
        string name = Enum.GetName(typeof(SoundObjects), currentObject);
        // Dafault 음성으로 바꾸기
        AudioManager.Instacne.SetSFX(name, AudioManager.Instacne.default_sfx[(int)currentObject-1].clip);
        aud.clip = AudioManager.Instacne.default_sfx[(int)currentObject - 1].clip;
    }


    public void PlayBtnClicked(PointerEventData data)
    {
        if (currentObject != SoundObjects.def)
        {
            if (aud.clip == null)
            {
                aud.clip = AudioManager.Instacne.sfx[(int)currentObject - 1].clip;
            }

            aud.Play();
            
        }
    }

    public void OffBlockImg()
    {
        blockImage.SetActive(false);
    }


    public void BackBtnClicked(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
        AudioManager.Instacne.SaveAudios();
    }

    public void SaveClip(PointerEventData data)
    {
        string name = Enum.GetName(typeof(SoundObjects), currentObject);

        if(currentObject != SoundObjects.def)
        {
            AudioManager.Instacne.SetSFX(name, aud.clip);
            //SavWav.Save("C:/Users/user/wkspaces/Elementee/Assets/Cherry/Records/" + name, aud.clip);
            //AudioManager에 저장
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

    public void Orange(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Orange);
    }

    public void Green(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Green);
    }

    public void Purple(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Purple);
    }

    public void Jump(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Jump);
    }

    public void Dash(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Dash);
    }

    public void Hurt(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Hurt);
    }

    public void Dead(PointerEventData data)
    {
        SetSoundObject(SoundObjects.Dead);
    }



    public void SceneJump(PointerEventData data)
    {
        //ClosePopupUI();
        //SceneManager.LoadScene(2);
        GameManager.Instance.RetryGame();

    }




}
