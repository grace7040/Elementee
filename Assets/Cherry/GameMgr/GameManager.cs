using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public bool isGameOver;


    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 게임오버 확인 (ex. 플레이어의 체력)
    }

    public void PauseGame()
    {
        // 게임 일시정지
        print("일시정지");
    }

    public void ResumeGame()
    {
        print("계속하자");
    }

    public void RetryGame()
    {
        // 게임 재시작
        print("재시작");
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            // 승리 UI 띄우기
            isGameOver = true;
        }
        else
        {
            // 패배 UI 띄우기
            isGameOver = true;
        }
    }

}
