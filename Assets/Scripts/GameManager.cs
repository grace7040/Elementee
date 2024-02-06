using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManage")]
    // JSON ����
    public List<int> mapStar = new List<int>();
    public int mapBest;  // �÷��� ������ ���� ū ��

    public int currentMapNum = 0;
    public GameObject Potal;
    public bool isGameOver;
    public int starCount = 0;

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
       // DataManager.Instance.JsonClear();
        DataManager.Instance.JsonLoad();
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
        starCount += 1;

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
        Managers.UI.ClosePopupUI();
    }

    public void NewGame()
    {
        starCount = 0;
        ResumeGame();
        // ���� �����
        SceneManager.LoadScene("Map_0");
    }

    public void RetryGame() // ���� �����
    {
        ResumeGame();
        isGameOver = false;

        // �÷��̾� �� �ʱ�ȭ + Color State �ʱ�ȭ
        playerHP = playerMAXHP;

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene("Map_" + currentMapNum);
        

    }

    public void GameOver()
    {
        // �й� UI ����
        isGameOver = true;
        Time.timeScale = 0;

        // �� �ȶ�̾�����?
        Managers.UI.ShowPopupUI<UI_GameOver>();

    }

    public void GameWin()
    {
        Time.timeScale = 0;

        // ��� ����
        if (mapBest<=currentMapNum) // ó�� Clear�� ���� ���
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

        // �¸� UI ���� 
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
