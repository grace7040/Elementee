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

    enum Buttons
    {
        SaveBtn,
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

        GetButton((int)Buttons.SaveBtn).gameObject.BindEvent(SaveBtnClicked);
        GetButton((int)Buttons.ResetBtn).gameObject.BindEvent(ResetBtnClicked);

    }

    public void SaveBtnClicked(PointerEventData data)
    {
        
        if (player != null)
        {
            player.GetComponent<PlayerController>().SetCustomWeapon();
            ClosePopupUI();

            ColorManager.Instance.SetPlayerCustomWeapon();
            GameManager.Instance.ResumeGame();

            DrawManager.Instance.CloseDrawing();
        }
        else  // ¾ó±¼ ±×¸± ¶§
           DrawManager.Instance.SaveFaceDrawing();

        Destroy(this);
   
    }

    public void ResetBtnClicked(PointerEventData data)
    {
        DrawManager.Instance.DrawbleObject.GetComponent<Drawable>().ResetCanvas();
    }


}
