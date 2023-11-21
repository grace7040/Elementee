using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManage")]
    public bool isGameOver;
    public int totalScore;

    // 이거 default 값 필요함. 나중에 맵 만들 때 추가할 것
    public float[] savePoint = new float[3];

    [Header("Player")]
    public Colors playerColor = Colors.def;
    public Sprite playerFace;

    public int playerHP = 100;
    public int playerMAXHP = 100;

    [Header("Item")]
    public Colors ReDrawItemColor = Colors.def;

    void Start()
    {
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

    public void PauseGame()
    {

        // 게임 일시정지
        Time.timeScale = 0;
            
        Managers.UI.ClosePopupUI();
        print("일시정지");

    }

    public void ResumeGame()
    {
        // 게임 다시 활성화
        Time.timeScale = 1;

        Managers.UI.ShowPopupUI<UI_InGame>();
        print("계속하자");
    }

    public void NewGame()
    {
        // DontDestroy 애들도 초기화 해줘야 할듯

        // 게임 재시작
        SceneManager.LoadScene(2);
    }

    public void RetryGame()
    {
        // 게임 재시작
        // 플레이어 피 초기화 + Color State 초기화
        playerHP = playerMAXHP;
        ColorManager.Instance.ResetColorState();

        SceneManager.LoadScene(2);

    }

    public void GameOver()
    {
        // 패배 UI 띄우기
        isGameOver = true;

        // 왜 안띄ㅜ어질까?
        Managers.UI.ShowPopupUI<UI_GameOver>();

    }

    public void GameWin()
    {
        // 승리 UI 띄우기
        isGameOver = true;
        PauseGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

}
