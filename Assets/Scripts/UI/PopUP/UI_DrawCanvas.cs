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
        color = GameManager.Instance.playerColor;
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkayBtn).gameObject.BindEvent(OkayBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnClicked);
        GetButton((int)Buttons.BasicBtn).gameObject.BindEvent(BasictBtnClicked);

        // �ȳ��� Text ����
        if(!DrawManager.Instance.face_mode)
            GuideText.text = DrawManager.Instance.DrawText[(int)color];
        else
            GuideText.text = DrawManager.Instance.DrawText[8];

    }

    public void OkayBtnClicked(PointerEventData data)
    {

        if (player != null)
        {
            player.GetComponent<PlayerController>().SetCustomWeapon();
            ClosePopupUI();

            GameManager.Instance.ResumeGame();
            DrawManager.Instance.CloseDrawing();

            // ���� �ٲٱ�
            if (basic_weapon)
            {
                ColorManager.Instance.SetPlayerBasicWeapon();
            }
            else
            {
                ColorManager.Instance.SetPlayerCustomWeapon();
            }

        }
        else  // �� �׸� ��
        {
            ClosePopupUI();
            DrawManager.Instance.SaveFaceDrawing();
        }

        // ���� �÷��̾�� ����
        ColorManager.Instance.SetOnPlayer(GameManager.Instance.playerColor);

    }

    //�⺻ ����� �����ϰ� ���� ��
    public void BasictBtnClicked(PointerEventData data)
    {
        // �׸����� ����� �⺻ ���� �����ֱ�
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
