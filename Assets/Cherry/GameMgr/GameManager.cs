using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public bool isGameOver;

    void Start()
    {
        isGameOver = false;
    }


    void Update()
    {
        // ���ӿ��� Ȯ�� (ex. �÷��̾��� ü��)
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
        print("�����");
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            // �¸� UI ����
            isGameOver = true;
        }
        else
        {
            // �й� UI ����
            isGameOver = true;
        }
    }

}
