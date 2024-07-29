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
    public int RecordTime = 1;
    public Sprite[] Sprites;
    public CustomSound CurrentObject;
    public GameObject BlockImage;

    public GameObject RecordAnim;

    AudioClip _record;
    AudioSource _aud;

    

    public enum CustomSound
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
        _aud = GetComponent<AudioSource>();

    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.Red).gameObject.BindEvent(Red);
        GetButton((int)Buttons.Yellow).gameObject.BindEvent(Yellow);
        GetButton((int)Buttons.Blue).gameObject.BindEvent(Blue);
        GetButton((int)Buttons.Green).gameObject.BindEvent(Green);
        GetButton((int)Buttons.Orange).gameObject.BindEvent(Orange);
        GetButton((int)Buttons.Purple).gameObject.BindEvent(Purple);
        GetButton((int)Buttons.Black).gameObject.BindEvent(Black);
        GetButton((int)Buttons.Jump).gameObject.BindEvent(Jump);
        GetButton((int)Buttons.Dash).gameObject.BindEvent(Dash);
        GetButton((int)Buttons.Dead).gameObject.BindEvent(Dead);
        GetButton((int)Buttons.Hurt).gameObject.BindEvent(Hurt);


        GetButton((int)Buttons.Save).gameObject.BindEvent(SaveClip);
        GetButton((int)Buttons.Record).gameObject.BindEvent(RecordBtnClicked);
        GetButton((int)Buttons.Play).gameObject.BindEvent(PlayBtnClicked);
        GetButton((int)Buttons.Default).gameObject.BindEvent(DafaultBtnClicked);
        GetButton((int)Buttons.Pause).gameObject.BindEvent(PauseBtnClicked);

        GetButton((int)Buttons.Back).gameObject.BindEvent(BackBtnClicked);

    }

    public void SetSoundObject(CustomSound obj)
    {
        CurrentObject = obj;
        GetImage((int)Images.Image).sprite = Sprites[(int)CurrentObject];
    }

    public void PauseBtnClicked(PointerEventData data)
    {
        _aud.Pause();
    }

    public void RecordBtnClicked(PointerEventData data)
    {
      
        if (CurrentObject != CustomSound.def)
        {
            RecordAnim.GetComponent<Animator>().Play("Record", -1, 0f);

            _record = Microphone.Start(Microphone.devices[0].ToString(), false, RecordTime, 44100);
            _aud.clip = _record;
            BlockImage.SetActive(true);
            Invoke("OffBlockImg", RecordTime);
        }


    }

    public void DafaultBtnClicked(PointerEventData data)
    {
        string name = Enum.GetName(typeof(CustomSound), CurrentObject);
        AudioManager.Instacne.SetSFX(name, AudioManager.Instacne.DefaultSfx[(int)CurrentObject-1].Clip);
        _aud.clip = AudioManager.Instacne.DefaultSfx[(int)CurrentObject - 1].Clip;
    }


    public void PlayBtnClicked(PointerEventData data)
    {
        if (CurrentObject != CustomSound.def)
        {
            if (_aud.clip == null)
            {
                _aud.clip = AudioManager.Instacne.Sfx[(int)CurrentObject - 1].Clip;
            }

            _aud.Play();
            
        }
    }

    public void OffBlockImg()
    {
        BlockImage.SetActive(false);
    }


    public void BackBtnClicked(PointerEventData data)
    {
        UIManager.Instance.ClosePopupUI();
        AudioManager.Instacne.SaveAudios();
    }

    public void SaveClip(PointerEventData data)
    {
        string name = Enum.GetName(typeof(CustomSound), CurrentObject);

        if(CurrentObject != CustomSound.def)
        {
            AudioManager.Instacne.SetSFX(name, _aud.clip);
        }

    }


    public void Red(PointerEventData data)
    {
        SetSoundObject(CustomSound.Red);
    }

    public void Yellow(PointerEventData data)
    {
        SetSoundObject(CustomSound.Yellow);
    }

    public void Blue(PointerEventData data)
    {
        SetSoundObject(CustomSound.Blue);
    }

    public void Orange(PointerEventData data)
    {
        SetSoundObject(CustomSound.Orange);
    }

    public void Green(PointerEventData data)
    {
        SetSoundObject(CustomSound.Green);
    }

    public void Purple(PointerEventData data)
    {
        SetSoundObject(CustomSound.Purple);
    }

    public void Black(PointerEventData data)
    {
        SetSoundObject(CustomSound.Black);
    }

    public void Jump(PointerEventData data)
    {
        SetSoundObject(CustomSound.Jump);
    }

    public void Dash(PointerEventData data)
    {
        SetSoundObject(CustomSound.Dash);
    }

    public void Hurt(PointerEventData data)
    {
        SetSoundObject(CustomSound.Hurt);
    }

    public void Dead(PointerEventData data)
    {
        SetSoundObject(CustomSound.Dead);
    }


}
