using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_GameWin : UI_Popup
{
    public List<GameObject> Stars = new List<GameObject>();
    enum Buttons
    {
        ToMainBtn,
        NextBtn,
        RetryBtn,

    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(ToMainBtnClicked);
        GetButton((int)Buttons.NextBtn).gameObject.BindEvent(ToNextBtnClicked);
        GetButton((int)Buttons.RetryBtn).gameObject.BindEvent(RetryBtnClicked);

        // 별 개수 반영 - 나중에 수식 추가 가능성 o
        for(int i=0; i<Stars.Count; i++)
        {
            // 개수 만큼 노란색 색칠
            if(i<GameManager.Instance.starCount)
                Stars[i].GetComponent<Image>().color = new Color32(255, 250, 99, 255);
            else
                Stars[i].GetComponent<Image>().color = new Color32(150, 150, 150, 255);
        }
    }


    public void RetryBtnClicked(PointerEventData data)
    {
        GameManager.Instance.RetryGame();
    }

    public void ToMainBtnClicked(PointerEventData data)
    {
        GameManager.Instance.GoToMainMenu();
    }
    public void ToNextBtnClicked(PointerEventData data)
    {
        GameManager.Instance.NextStage();
    }
}