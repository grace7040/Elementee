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
        CheckPurchaseable();
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

    public void CheckPurchaseable()
    {
        for (int i = 0; i < ShopItemsSO.Length; i++)
        {
            // Coin이 부족한경우
            if (TotalCoin >= ShopItemsSO[i].BaseCost)
            {
                MyPurchaseBtns[i].interactable = true;
                ShopPanels[i].BlockOkayBtn.SetActive(false);
            }
            else
            {
                MyPurchaseBtns[i].interactable = false;
                ShopPanels[i].BlockOkayBtn.SetActive(true);
            }

            // 이미 구매 완료한 경우
            if (GameManager.Instance.ShopItemPurchaseList.Contains(ShopItemsSO[i]))
            {
                ShopPanels[i].PurchasedUI.SetActive(true);
            }
        }

    }

    public void PurchaseItem(int btnNo)
    {
        if(TotalCoin >= ShopItemsSO[btnNo].BaseCost)
        {
            TotalCoin -= ShopItemsSO[btnNo].BaseCost;
            GameManager.Instance.TotalCoin = TotalCoin;
            GameManager.Instance.ShopItemPurchaseList.Add(ShopItemsSO[btnNo]);
            GameManager.Instance.CurrentShopItemSO = ShopItemsSO[btnNo];
            DataManager.Instance.JsonSave();
            CoinUI.text = TotalCoin.ToString();

            CheckPurchaseable();
        }
    }

    public void LoadPanels()
    {
        for(int i =0; i < ShopItemsSO.Length; i++)
        {
            ShopPanels[i].TitleText.text = ShopItemsSO[i].Title;
            ShopPanels[i].DescriptionText.text = ShopItemsSO[i].Description;
            ShopPanels[i].CostText.text = ShopItemsSO[i].BaseCost.ToString();
            ShopPanels[i].ItemSprite.sprite = ShopItemsSO[i].ItemSprite;
        }
    }
    

}
