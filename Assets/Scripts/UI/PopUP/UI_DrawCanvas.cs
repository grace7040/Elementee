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
    GameObject player;
    public TextMeshProUGUI GuideText;

    bool basic_weapon = false;
    private Colors color;

    enum Buttons
    {
        OkayBtn,
        ResetBtn,
        ExitBtn,
        BasicBtn,

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        color = GameManager.Instance.PlayerColor;
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
        if (!DrawManager.Instance.face_mode)
            GuideText.text = DrawManager.Instance.DrawText[(int)color];
        else
            GuideText.text = DrawManager.Instance.DrawText[8];

    }

    public void OkayBtnClicked(PointerEventData data)
    {

        if (player != null)
        {
            player.GetComponent<PlayerAttack>().SetCustomWeapon();
            ClosePopupUI();

            GameManager.Instance.ResumeGame();
            DrawManager.Instance.CloseDrawing();

            // 무기 바꾸기
            if (basic_weapon)
            {
                ColorManager.Instance.SetPlayerBasicWeapon();
            }
            else
            {
                ColorManager.Instance.SetPlayerCustomWeapon();
            }

        }
        else  // 얼굴 그릴 때
        {
            ClosePopupUI();
            DrawManager.Instance.SaveFaceDrawing();
        }

        // 무기 플레이어에게 적용
        ColorManager.Instance.SetOnPlayer(GameManager.Instance.PlayerColor);

    }

    //기본 무기로 적용하고 싶을 때
    public void BasictBtnClicked(PointerEventData data)
    {
        // 그리던거 지우고 기본 무기 보여주기
        basic_weapon = true;
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
        DrawManager.Instance.BasicWeapons(1);

    }


    public void ResetBtnClicked(PointerEventData data)
    {
        basic_weapon = false;
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
        DrawManager.Instance.BasicWeapons(0);
    }


}