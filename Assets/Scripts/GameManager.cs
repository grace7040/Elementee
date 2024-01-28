using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManage")]
    public bool isGameOver;
    public int totalScore;
    public int starCount = 10;

    // �̰� default �� �ʿ���. ���߿� �� ���� �� �߰��� ��
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

    public void StarCount()
    {
        starCount -= 1;
        if(starCount == 0)
        {
            GameWin();
        }

    }

    public void PauseGame()
    {

        // ���� �Ͻ�����
        Time.timeScale = 0;
            
        Managers.UI.ClosePopupUI();

    }

    public void ResumeGame()
    {
        // ���� �ٽ� Ȱ��ȭ
        Time.timeScale = 1;

    }

    public void NewGame()
    {

        Time.timeScale = 1;
        // ���� �����
        SceneManager.LoadScene(1);
    }

    public void RetryGame()
    {
        isGameOver = false;

        // ���� �����
        // �÷��̾� �� �ʱ�ȭ + Color State �ʱ�ȭ
        playerHP = playerMAXHP;
        ColorManager.Instance.ResetColorState();

        SceneManager.LoadScene(1);
        Time.timeScale = 1;

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
        Managers.UI.ShowPopupUI<UI_GameWin>();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
