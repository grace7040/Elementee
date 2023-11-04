using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public bool isGameOver;
    public int playerHP = 100;
    public int playerMAXHP = 100;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        // ���ӿ��� Ȯ�� (ex. �÷��̾��� ü��)
    }

    public float HPBar()
    {
        return (float)playerHP / playerMAXHP;
    }

    public void PauseGame()
    {
        
        // ���� �Ͻ�����
        Managers.UI.ClosePopupUI();
        print("�Ͻ�����");
    }

    public void ResumeGame()
    {
        Managers.UI.ShowPopupUI<UI_InGame>();
        print("�������");
    }

    public void RetryGame()
    {
        // ���� �����
        SceneManager.LoadScene(2);
    }

    public void GameOver()
    {
        
        // �й� UI ����
        isGameOver = true;

        // �� �ȶ�̾�����?
        Managers.UI.ShowPopupUI<UI_GameOver>();

       
    }

    public void GameWin()
    {
        // �¸� UI ����
        isGameOver = true;
        PauseGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

}
