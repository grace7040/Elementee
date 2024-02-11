using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_LevelMenu : UI_Scene
{
    public List<GameObject> mapBtns = new List<GameObject>();
    public List<GameObject> Lines = new List<GameObject>();
    
    // Line 관련
    private Image lineImg;
    private float duration = 1.5f; // Line 채워지는 시간
    private float endFillamount = 1f;

    private int mapBest;

    enum Buttons
    {
        Lobby,
    }


    private void Start()
    {
        mapBest = GameManager.Instance.mapBest;
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobbyBtn);

        for(int i=0; i<mapBtns.Count; i++)
        {
            if (GameManager.Instance.developMode || i < mapBest)
            {
                mapBtns[i].GetComponent<Button>().interactable = true;
                Lines[i].SetActive(true);
            }
            if(i == mapBest)
            {
                Lines[i].SetActive(true);
                lineImg = Lines[i].GetComponent<Image>();
                endFillamount = lineImg.fillAmount;
                StartCoroutine(ChangeFillAmountOverTime());

            }

        }

    }

    private IEnumerator ChangeFillAmountOverTime()
    {
        float currentTime = 0.0f;
        float startFillAmount = 0.0f;

        while (currentTime < duration)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, endFillamount, currentTime / duration);
            fillAmount = Mathf.Clamp01(fillAmount);

            lineImg.fillAmount = fillAmount;
            currentTime += Time.deltaTime;

            yield return null;
        }

        lineImg.fillAmount = endFillamount;
        mapBtns[mapBest].GetComponent<Button>().interactable = true;
    }


    public void ToLobbyBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");
    }

}
