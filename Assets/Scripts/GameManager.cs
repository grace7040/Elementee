using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool developMode;

    [Header("GameManage")]
    // JSON 저장될 Data
    public List<int> MapStar = new List<int>();
    public int MapBest;
    public int MapCoin = 0;

    public int CurrentMapNum = 0;
    public int CurrentStar = 0;
    public GameObject CurrentPotal;
    public UI_Game UIGame;
    public bool IsFirstPlay = true;

    [Header("Player")]
    public GameObject Player;
    //public Transform sponPos;
    public Colors PlayerColor = Colors.Default;
    public Sprite PlayerFace;


    public delegate void Del();
    //public Del SetJoystick = null;

    [Header("Item")]
    public int TotalCoin;
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
        InitGame();

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    

    }

    public void InitGame()
    {
        MapCoin = 0;
        IsFirstPlay = true;

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowPopupUI<UI_GameOver>();
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        TotalCoin += MapCoin;

        // Data Save
        if (MapBest <= CurrentMapNum)
        {
            MapStar.Add(CurrentStar);
            //mapFlag.Add(0);
            MapBest += 1;
        }
        else
        {
            if (MapStar[CurrentMapNum] < CurrentStar)
            {
                MapStar[CurrentMapNum] = CurrentStar;
            }
        }
        DataManager.Instance.JsonSave();

        UIManager.Instance.ShowPopupUI<UI_GameWin>();
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
        Player.GetComponent<PlayerController>().Revival();
        IsFirstPlay = false;
    }
}