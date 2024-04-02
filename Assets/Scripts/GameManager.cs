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
   // public List<int> mapFlag = new List<int>();
    public List<Vector3> yehh = new List<Vector3>();
    public int mapBest;  // 플레이 가능한 가장 큰 맵
    public int mapCoin = 0;


    public int currentMapNum = 0;
    public GameObject Potal;
    public int starCount = 0;
    public bool isFirst = true;

    [Header("Player")]
    public GameObject player;
    public Transform sponPos;
    public Colors playerColor = Colors.def;
    public Sprite playerFace;

    public int playerHP = 200;
    public int playerMAXHP = 200;

    public delegate void Del();
    //public Del SetJoystick = null;

    [Header("Item")]
    public int coin;
    public Colors ReDrawItemColor = Colors.def;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //DataManager.Instance.JsonClear(); // 데이터 초기화
        DataManager.Instance.JsonLoad();
    }

    public float HPBar()
    {
        return (float)playerHP / playerMAXHP;
    }

    public void StarCount()
    {
        starCount += 1;
    }

    public void PauseGame()
    {
        // 게임 일시정지
        Time.timeScale = 0;
        Managers.UI.ClosePopupUI();
    }

    public void ResumeGame()
    {
        // 게임 다시 활성화
        Time.timeScale = 1;
        Managers.UI.ClosePopupUI();
    }

    public void NewGame()
    {
        starCount = 0;
        mapCoin = 0;
        ResumeGame();
        SceneManager.LoadScene("Map_0");
        DataManager.Instance.JsonClear(); // 데이터 초기화

    }

    public void RetryGame() // 게임 재시작
    {
        ResumeGame();
        mapCoin = 0;
        isFirst = true;

        // 플레이어 피 초기화 + Color State 초기화
        playerHP = playerMAXHP;

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재시작        

    }

    public void GameOver()
    {
        // 패배 UI 띄우기
        Time.timeScale = 0;
        Managers.UI.ShowPopupUI<UI_GameOver>();

    }

    public void GameWin()
    {
        Time.timeScale = 0;
        coin += mapCoin;

        // 기록 저장
        if (mapBest<=currentMapNum) // 처음 Clear한 맵일 경우
        {
            mapStar.Add(starCount);
            //mapFlag.Add(0);
            mapBest += 1;
        }
        else
        {
            if (mapStar[currentMapNum] < starCount)
            {
                mapStar[currentMapNum] = starCount;
                //mapFlag[currentMapNum] = 0;

            }
        }
        DataManager.Instance.JsonSave();

        // 승리 UI 띄우기 
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
        playerHP = playerMAXHP;
        ResumeGame();
        player.GetComponent<PlayerController>().Revival();
        isFirst = false;
    }

    


}
