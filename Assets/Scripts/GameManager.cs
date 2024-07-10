using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool developMode;

    [Header("GameManage")]
    // JSON 저장
    public List<int> mapStar = new List<int>();
    public int mapBest;
    public int mapCoin = 0;

    public int currentMapNum = 0;
    public int currentStar = 0;
    public GameObject currentPotal;
    public bool isFirstPlay = true;

    [Header("Player")]
    public GameObject player;
    //public Transform sponPos;
    public Colors playerColor = Colors.Default;
    public Sprite playerFace;


    public delegate void Del();
    //public Del SetJoystick = null;

    [Header("Item")]
    public int totalCoin;
    public Colors ReDrawItemColor = Colors.Default;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //DataManager.Instance.JsonClear(); // 데이터 초기화
        DataManager.Instance.JsonLoad();
    }

    public void StarCount()
    {
        currentStar += 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Managers.UI.ClosePopupUI();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Managers.UI.ClosePopupUI();
    }

    public void RetryGame()
    {
        ResumeGame();
        InitGame();

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    

    }

    public void InitGame()
    {
        mapCoin = 0;
        isFirstPlay = true;

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Managers.UI.ShowPopupUI<UI_GameOver>();
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        totalCoin += mapCoin;

        // Data Save
        if (mapBest <= currentMapNum)
        {
            mapStar.Add(currentStar);
            //mapFlag.Add(0);
            mapBest += 1;
        }
        else
        {
            if (mapStar[currentMapNum] < currentStar)
            {
                mapStar[currentMapNum] = currentStar;
            }
        }
        DataManager.Instance.JsonSave();

        Managers.UI.ShowPopupUI<UI_GameWin>();
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Lobby");
    }

    public void NextStage()
    {
        currentMapNum += 1;
        ResumeGame();
        SceneManager.LoadScene("Map_" + currentMapNum);
    }

    public void Revival()
    {
        ResumeGame();
        player.GetComponent<PlayerController>().Revival();
        isFirstPlay = false;
    }
}