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

    enum Buttons
    {
        OkayBtn,
        ResetBtn,
        ExitBtn,

    }

    private void Start()
    {
        Init();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnClicked);

        if (DrawManager.Instance.face_mode)
        {
            print("안녕");
            GuideText.text = "얼굴에 맞게 표정을 그려주세요";
        }
    }

    public void OkayBtnClicked(PointerEventData data)
    {

        if (player != null)
        {
            player.GetComponent<PlayerController>().SetCustomWeapon();
            ClosePopupUI();

            ColorManager.Instance.SetPlayerCustomWeapon();
            GameManager.Instance.ResumeGame();
            DrawManager.Instance.CloseDrawing();
        }
        else  // 얼굴 그릴 때
        {
            ClosePopupUI();
            DrawManager.Instance.SaveFaceDrawing();
        }

        // 플레이어에게 적용
        ColorManager.Instance.SetOnPlayer(GameManager.Instance.playerColor);

    }

    public void ResetBtnClicked(PointerEventData data)
    {
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
    }


}
