using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Shop : UI_Popup
{
    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    enum Buttons
    {
        Lobby,

    }


    private void Start()
    {
        Init();

        for(int i=0; i<shopItemsSO.Length; i++)
            shopPanelsGO[i].SetActive(true);

        coins = GameManager.Instance.coin;
        coinUI.text = coins.ToString();
        LoadPanels();
       // CheckPurchaseable(); -> 살 수 없을 경우 버튼 막기
    }

    public override void Init()
    {
        base.Init(); // 📜UI_Button 의 부모인 📜UI_PopUp 의 Init() 호출

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Lobby).gameObject.BindEvent(ToLobby);

    }

    public void ToLobby(PointerEventData data)
    {
        SceneManager.LoadScene("Lobby");
    }

    //public void CheckPurchaseable()
    //{
    //    for(int i=0; i< shopItemsSO.Length; i++)
    //    {
    //        if (coins >= shopItemsSO[i].baseCost)
    //            myPurchaseBtns[i].interactable = true;
    //        else
    //            myPurchaseBtns[i].interactable = false;
    //    }

    //}

    public void PurchaseItem(int btnNo)
    {
        if(coins >= shopItemsSO[btnNo].baseCost)
        {
            coins -= shopItemsSO[btnNo].baseCost;
            GameManager.Instance.coin = coins;
            DataManager.Instance.JsonSave(); // Coin 사용 후 저장
            coinUI.text = coins.ToString();
           // CheckPurchaseable();

        }
    }

    public void LoadPanels()
    {
        for(int i =0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleText.text = shopItemsSO[i].title;
            shopPanels[i].descriptionText.text = shopItemsSO[i].description;
            shopPanels[i].costText.text = shopItemsSO[i].baseCost.ToString();
        }
    }
    

}
