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
    public Button[] MySelectBtns;

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

            // 이미 구매한 경우
            if(GameManager.Instance.CurrentShopItemSO == ShopItemsSO[i])
            {
                ShopPanels[i].SelectedUI.SetActive(true);
                ShopPanels[i].PurchasedUI.SetActive(false);
                MySelectBtns[i].interactable = true;

            }
            else
            {
                if (GameManager.Instance.ShopItemPurchaseList.Contains(ShopItemsSO[i]))
                {
                    ShopPanels[i].PurchasedUI.SetActive(true);
                    ShopPanels[i].SelectedUI.SetActive(false);
                    MySelectBtns[i].interactable = true;

                }
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
            DataManager.Instance.JsonSave();
            CoinUI.text = TotalCoin.ToString();
            MySelectBtns[btnNo].interactable = true;

            CheckPurchaseable();
        }
    }

    public void SelectItem(int btnNo)
    {
        GameManager.Instance.CurrentShopItemSO = ShopItemsSO[btnNo];
        DataManager.Instance.JsonSave();
        CheckPurchaseable();
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
