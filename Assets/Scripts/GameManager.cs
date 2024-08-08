using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool developMode;

    [Header("GameManage")]
    // JSON 저장될 Data
    public List<int> StarCountsPerMap = new List<int>();
    public int CompletedMap;
    public int TotalCoin;

    [Header("ShopItem")]
    public List<ShopItemSO> ShopItemPurchaseList = new List<ShopItemSO>();
    public ShopItemSO CurrentShopItemSO;

    [Header("CurrentData")]
    public int CurrentCoin = 0;
    public int CurrentMapNum = 0;
    public int CurrentStar = 0;
    public GameObject CurrentPotal;
    public UI_Game UIGame;
    bool _hasBeenRevived = false;
    public bool CanRevival
    {
        get { return (!_hasBeenRevived && (RevivalPos != null)); }
    }

    [Header("Player")]
    public GameObject Player;
    public Transform RevivalPos;
    public Colors PlayerColor = Colors.Default;
    public Sprite PlayerFace;
    public List<Sprite> CurrentWeaponSpriteList = new();

    public Action SaveCurrentWeaponSprite = null;

    public delegate void Del();
    //public Del SetJoystick = null;

    [Header("Item")]
    public Colors ReDrawItemColor = Colors.Default;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DataManager.Instance.JsonLoad();
        DataManager.Instance.JsonLoadWeaponSprites();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby" || scene.name == "LevelMenu")
            return;

        InitGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        UIManager.Instance.ClosePopupUI();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        UIManager.Instance.ClosePopupUI();
    }

    public void RetryGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InitGame()
    {
        CurrentCoin = 0;
        _hasBeenRevived = false;
        RevivalPos = null;
        ColorManager.Instance.Init();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowPopupUI<UI_GameOver>();
    }

    public void GameWin()
    {
        UIManager.Instance.ShowPopupUI<UI_GameWin>();

        Time.timeScale = 0;
        TotalCoin += CurrentCoin;

        // Data Save
        if (CompletedMap <= CurrentMapNum)
        {
            StarCountsPerMap.Add(CurrentStar);
            CompletedMap += 1;
        }
        else
        {
            if (StarCountsPerMap[CurrentMapNum] < CurrentStar)
            {
                StarCountsPerMap[CurrentMapNum] = CurrentStar;
            }
        }
        SaveCurrentWeaponSprite?.Invoke();
        DataManager.Instance.JsonSave();
        DataManager.Instance.JsonSaveWeaponSprites();

        CurrentStar = 0;
        CurrentCoin = 0;
    }


    public void GoToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Lobby");
    }

    public void NextStage()
    {
        CurrentMapNum += 1;
        ResumeGame();
        SceneManager.LoadScene("Map_" + CurrentMapNum);
    }

    public void Revival()
    {
        ResumeGame();
        Player.GetComponent<PlayerController>().Revive(RevivalPos.position);
        _hasBeenRevived = true;
    }
}