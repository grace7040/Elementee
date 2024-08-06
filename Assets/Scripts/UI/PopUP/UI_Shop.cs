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
    public int TotalCoin;
    public TMP_Text CoinUI;
    public ShopItemSO[] ShopItemsSO;
    public GameObject[] ShopPanelsGO;
    public ShopTemplate[] ShopPanels;
    public Button[] MyPurchaseBtns;

    enum Buttons
    {
        Lobby,
    }


    private void Start()
    {
        Init();

        for(int i=0; i<ShopItemsSO.Length; i++)
            ShopPanelsGO[i].SetActive(true);

        TotalCoin = GameManager.Instance.TotalCoin;
        CoinUI.text = TotalCoin.ToString();
        LoadPanels();
       // CheckPurchaseable(); -> 살 수 없을 경우 버튼 막기
    }

    public override void Init()
    {
        base.Init(); 

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
        if(TotalCoin >= ShopItemsSO[btnNo].baseCost)
        {
            TotalCoin -= ShopItemsSO[btnNo].baseCost;
            GameManager.Instance.TotalCoin = TotalCoin;
            DataManager.Instance.JsonSave();
            CoinUI.text = TotalCoin.ToString();
           // CheckPurchaseable();

        }
    }

    public void LoadPanels()
    {
        for(int i =0; i < ShopItemsSO.Length; i++)
        {
            ShopPanels[i].titleText.text = ShopItemsSO[i].title;
            ShopPanels[i].descriptionText.text = ShopItemsSO[i].description;
            ShopPanels[i].costText.text = ShopItemsSO[i].baseCost.ToString();
        }
    }
    

}
