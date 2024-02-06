using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData // ������ ������
{
    public List<int> mapStar = new List<int>();  // �� �� ȹ���� Star ����
    public int mapBest; // �÷��� ������ �� �� Best
}

public class DataManager : Singleton<DataManager>
{
    private string path;
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
       // JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();
        path = Path.Combine(Application.persistentDataPath, "database.json");

        // ���� ��� �� �ʱ�ȭ
        if (!File.Exists(path))
        {
            GameManager.Instance.mapBest = 0;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                for (int i = 0; i < saveData.mapStar.Count; i++)
                {
                    GameManager.Instance.mapStar.Add(saveData.mapStar[i]);
                }
                GameManager.Instance.mapBest = saveData.mapBest;
            }
        }
    }

    public void JsonSave()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");

        // ������ �ʱ�ȭ
        SaveData saveData = new SaveData();

        // ������ �ε�
        for (int i = 0; i < GameManager.Instance.mapStar.Count; i++)
        {
            saveData.mapStar.Add(GameManager.Instance.mapStar[i]);
        }
        saveData.mapBest = GameManager.Instance.mapBest;

        // ������ ����
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void JsonClear() // �׽�Ʈ�� ������ �ʱ�ȭ
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();
        saveData.mapBest = 0;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

}
