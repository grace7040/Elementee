using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData // 저장할 데이터
{
    public List<int> mapStar = new List<int>();
    //public List<int> mapFlag = new List<int>();
    public int mapBest;
    public int Coin;
}

public class DataManager : Singleton<DataManager>
{
    private string path;
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();
        path = Path.Combine(Application.persistentDataPath, "database.json");

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
                    //GameManager.Instance.mapFlag.Add(saveData.mapFlag[i]);
                }
                GameManager.Instance.mapBest = saveData.mapBest;
                GameManager.Instance.totalCoin = saveData.Coin;
            }
        }
    }

    public void JsonSave()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();

        // Data Load
        for (int i = 0; i < GameManager.Instance.mapStar.Count; i++)
        {
            saveData.mapStar.Add(GameManager.Instance.mapStar[i]);
            //saveData.mapFlag.Add(GameManager.Instance.mapFlag[i]);
        }
        saveData.mapBest = GameManager.Instance.mapBest;
        saveData.Coin = GameManager.Instance.totalCoin;

        // Data Save
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void JsonClear() // Clear Data
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();
        saveData.mapBest = 0;
        saveData.Coin = 0;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

}
