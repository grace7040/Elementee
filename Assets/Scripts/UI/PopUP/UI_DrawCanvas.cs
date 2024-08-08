using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using FreeDraw;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_DrawCanvas : UI_Popup
{
    GameObject _player;
    public TextMeshProUGUI GuideText;

    bool _useBasicWeapon = false;
    Colors _color;

    enum Buttons
    {
        OkayBtn,
        ResetBtn,
        ExitBtn,
        BasicBtn,

    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _color = GameManager.Instance.PlayerColor;
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnClicked);
        GetButton((int)Buttons.BasicBtn).gameObject.BindEvent(BasictBtnClicked);

        // 안내문 Text 설정
        if (!DrawManager.Instance.IsDrawingFace)
            GuideText.text = DrawManager.Instance.DrawText[(int)_color];
        else
            GuideText.text = DrawManager.Instance.DrawText[8];

    }

    public void OkayBtnClicked(PointerEventData data)
    {
        if (_player != null)
        {
            if (!_useBasicWeapon)
            {
                if (!DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().HasDrawn)
                {
                    Debug.Log("무기 그리고 다시 ok 누르삼");
                    return;
                }
                _player.GetComponent<PlayerAttack>().SetCustomWeapon();
            }
            else
            {
                _player.GetComponent<PlayerAttack>().SetBasicWeapon();
            }
            
            ClosePopupUI();

            GameManager.Instance.ResumeGame();
            DrawManager.Instance.CloseDrawing();

            // 무기 바꾸기
            GameManager.Instance.SaveCurrentWeaponSprite();
            ColorManager.Instance.SetColorState(GameManager.Instance.PlayerColor);

        }
        else  // 얼굴 그릴 때
        {
            ClosePopupUI();
            DrawManager.Instance.SaveFaceDrawingOnDevice();
          //  DrawManager.Instance.SaveWeapon((int)_color);
        }

    }

    //기본 무기로 적용하고 싶을 때
    public void BasictBtnClicked(PointerEventData data)
    {
        // 그리던거 지우고 기본 무기 보여주기
        _useBasicWeapon = true;
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
        DrawManager.Instance.UseBasicWeapons(_useBasicWeapon);

    }


    public void ResetBtnClicked(PointerEventData data)
    {
        _useBasicWeapon = false;
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
        DrawManager.Instance.UseBasicWeapons(_useBasicWeapon);
    }


}