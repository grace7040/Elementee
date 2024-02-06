using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManage")]
    // JSON 저장
    public List<int> mapStar = new List<int>();
    public int mapBest;  // 플레이 가능한 가장 큰 맵

    public int currentMapNum = 0;
    public GameObject Potal;
    public bool isGameOver;
    public int starCount = 0;

    // 이거 default 값 필요함. 나중에 맵 만들 때 추가할 것
    public float[] savePoint = new float[3];

    [Header("Player")]
    public Colors playerColor = Colors.def;
    public Sprite playerFace;

    public int playerHP = 200;
    public int playerMAXHP = 200;

    public delegate void Del();
    public Del SetJoystick = null;

    [Header("Item")]
    public Colors ReDrawItemColor = Colors.def;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
       // DataManager.Instance.JsonClear();
        DataManager.Instance.JsonLoad();
        isGameOver = false;
    }
    
    void Update()
    {
        // 게임오버 확인 (ex. 플레이어의 체력)
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
        ResumeGame();
        // 게임 재시작
        SceneManager.LoadScene("Map_0");
    }

    public void RetryGame() // 게임 재시작
    {
        ResumeGame();
        isGameOver = false;

        // 플레이어 피 초기화 + Color State 초기화
        playerHP = playerMAXHP;

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene("Map_" + currentMapNum);
        

    }

    public void GameOver()
    {
        // 패배 UI 띄우기
        isGameOver = true;
        Time.timeScale = 0;

        // 왜 안띄ㅜ어질까?
        Managers.UI.ShowPopupUI<UI_GameOver>();

    }

    public void GameWin()
    {
        Time.timeScale = 0;

        // 기록 저장
        if (mapBest<=currentMapNum) // 처음 Clear한 맵일 경우
        {
            mapStar.Add(starCount);
            mapBest += 1;
        }
        else
        {
            if(mapStar[currentMapNum]<starCount)
                mapStar[currentMapNum] = starCount;
        }
        DataManager.Instance.JsonSave();

        // 승리 UI 띄우기 
        isGameOver = true;
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

}
