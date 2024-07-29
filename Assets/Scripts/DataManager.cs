using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<int> MapStar = new List<int>();
    //public List<int> mapFlag = new List<int>();
    public int MapBest;
    public int TotalCoin;
}

public class DataManager : Singleton<DataManager>
{
    string _path;
    void Start()
    {
        _path = Path.Combine(Application.persistentDataPath, "database.json");
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();
        _path = Path.Combine(Application.persistentDataPath, "database.json");

        if (!File.Exists(_path))
        {
            GameManager.Instance.MapBest = 0;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(_path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                for (int i = 0; i < saveData.MapStar.Count; i++)
                {
                    GameManager.Instance.MapStar.Add(saveData.MapStar[i]);
                    //GameManager.Instance.mapFlag.Add(saveData.mapFlag[i]);
                }
                GameManager.Instance.MapBest = saveData.MapBest;
                GameManager.Instance.TotalCoin = saveData.TotalCoin;
            }
        }
    }

    public void JsonSave()
    {
        _path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();

        // Data Load
        for (int i = 0; i < GameManager.Instance.MapStar.Count; i++)
        {
            saveData.MapStar.Add(GameManager.Instance.MapStar[i]);
            //saveData.mapFlag.Add(GameManager.Instance.mapFlag[i]);
        }
        saveData.MapBest = GameManager.Instance.MapBest;
        saveData.TotalCoin = GameManager.Instance.TotalCoin;

        // Data Save
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_path, json);
    }

    public void JsonClear() // Clear Data
    {
        _path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();
        saveData.MapBest = 0;
        saveData.TotalCoin = 0;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_path, json);
    }

}
