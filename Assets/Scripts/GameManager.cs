using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool developMode;

    [Header("GameManage")]
    // JSON ����
    public List<int> mapStar = new List<int>();
   // public List<int> mapFlag = new List<int>();
    public List<Vector3> yehh = new List<Vector3>();
    public int mapBest;  // �÷��� ������ ���� ū ��
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
        //DataManager.Instance.JsonClear(); // ������ �ʱ�ȭ
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
        mapCoin = 0;
        ResumeGame();
        SceneManager.LoadScene("Map_0");
        DataManager.Instance.JsonClear(); // ������ �ʱ�ȭ

    }

    public void RetryGame() // ���� �����
    {
        ResumeGame();
        mapCoin = 0;
        isFirst = true;

        // �÷��̾� �� �ʱ�ȭ + Color State �ʱ�ȭ
        playerHP = playerMAXHP;

        ColorManager.Instance.ResetColorState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� �����        

    }

    public void GameOver()
    {
        // �й� UI ����
        Time.timeScale = 0;
        Managers.UI.ShowPopupUI<UI_GameOver>();

    }

    public void GameWin()
    {
        Time.timeScale = 0;
        coin += mapCoin;

        // ��� ����
        if (mapBest<=currentMapNum) // ó�� Clear�� ���� ���
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

        // �¸� UI ���� 
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
