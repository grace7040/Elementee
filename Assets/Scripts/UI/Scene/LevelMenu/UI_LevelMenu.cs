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
    public List<GameObject> MapBtns = new List<GameObject>();
    public List<GameObject> Lines = new List<GameObject>();
    
    // Line
    Image _lineImg;
    float _duration = 1.5f; // Line 채워지는 시간
    float _endFillamount = 1f;

    int _mapBest;

    enum Buttons
    {
        Lobby,
    }


    private void Start()
    {
        _mapBest = GameManager.Instance.MapBest;
        Init();
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobbyBtn);

        for(int i=0; i<MapBtns.Count; i++)
        {
            if (GameManager.Instance.developMode || i < _mapBest)
            {
                MapBtns[i].GetComponent<Button>().interactable = true;
                Lines[i].SetActive(true);
            }
            if(i == _mapBest)
            {
                Lines[i].SetActive(true);
                _lineImg = Lines[i].GetComponent<Image>();
                _endFillamount = _lineImg.fillAmount;
                StartCoroutine(ChangeFillAmountOverTime());

            }

        }

    }

    private IEnumerator ChangeFillAmountOverTime()
    {
        float currentTime = 0.0f;
        float startFillAmount = 0.0f;

        while (currentTime < _duration)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, _endFillamount, currentTime / _duration);
            fillAmount = Mathf.Clamp01(fillAmount);

            _lineImg.fillAmount = fillAmount;
            currentTime += Time.deltaTime;

            yield return null;
        }

        _lineImg.fillAmount = _endFillamount;
        MapBtns[_mapBest].GetComponent<Button>().interactable = true;
    }


    public void ToLobbyBtn(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");
    }

}
