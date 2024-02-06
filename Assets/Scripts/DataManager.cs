using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData // 저장할 데이터
{
    public List<int> mapStar = new List<int>();  // 맵 별 획득한 Star 개수
    public int mapBest; // 플레이 가능한 맵 중 Best
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

        // 없을 경우 값 초기화
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

        // 데이터 초기화
        SaveData saveData = new SaveData();

        // 데이터 로드
        for (int i = 0; i < GameManager.Instance.mapStar.Count; i++)
        {
            saveData.mapStar.Add(GameManager.Instance.mapStar[i]);
        }
        saveData.mapBest = GameManager.Instance.mapBest;

        // 데이터 저장
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void JsonClear() // 테스트용 데이터 초기화
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");

        SaveData saveData = new SaveData();
        saveData.mapBest = 0;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

}
