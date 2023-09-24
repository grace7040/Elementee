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
        // ���ӿ��� Ȯ�� (ex. �÷��̾��� ü��)
    }

    public void PauseGame()
    {
        // ���� �Ͻ�����
        print("�Ͻ�����");
    }

    public void ResumeGame()
    {
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
