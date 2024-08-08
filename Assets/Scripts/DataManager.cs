using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

[System.Serializable]
public class SaveData
{
    public List<int> StarCountsPerMap = new();
    public int CompletedMap;
    public int TotalCoin;
    public List<ShopItemSO> ShopItemPurchaseList = new();
    public ShopItemSO CurrentShopItemSO;
}

public class WeaponSprites
{
    public List<Sprite> CurrentWeaponSpriteList;
}

public class DataManager : Singleton<DataManager>
{
    const string _dbFileName = "database.json";
    const string _weaponSpriteFileName = "sprites.json";
    public void JsonLoad()
    {
        SaveData saveData = new SaveData();
        var path = Path.Combine(Application.persistentDataPath, _dbFileName);

        if (!File.Exists(path))
        {
            GameManager.Instance.CompletedMap = 0;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                for (int i = 0; i < saveData.StarCountsPerMap.Count; i++)
                {
                    GameManager.Instance.StarCountsPerMap.Add(saveData.StarCountsPerMap[i]);
                }
                for (int i = 0; i < saveData.ShopItemPurchaseList.Count; i++)
                {
                    GameManager.Instance.ShopItemPurchaseList.Add(saveData.ShopItemPurchaseList[i]);
                }
                GameManager.Instance.CompletedMap = saveData.CompletedMap;
                GameManager.Instance.TotalCoin = saveData.TotalCoin;
                GameManager.Instance.CurrentShopItemSO = saveData.CurrentShopItemSO;
            }
        }
    }
    public void JsonSave()
    {
        var path = Path.Combine(Application.persistentDataPath, _dbFileName);

        SaveData saveData = new SaveData();

        // Data Load
        for (int i = 0; i < GameManager.Instance.StarCountsPerMap.Count; i++)
        {
            saveData.StarCountsPerMap.Add(GameManager.Instance.StarCountsPerMap[i]);
        }
        for (int i = 0; i < GameManager.Instance.ShopItemPurchaseList.Count; i++)
        {
            Debug.Log(GameManager.Instance.ShopItemPurchaseList.Count);
            saveData.ShopItemPurchaseList.Add(GameManager.Instance.ShopItemPurchaseList[i]);
        }
        saveData.CompletedMap = GameManager.Instance.CompletedMap;
        saveData.TotalCoin = GameManager.Instance.TotalCoin;
        saveData.CurrentShopItemSO = GameManager.Instance.CurrentShopItemSO;

        // Data Save
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void JsonLoadWeaponSprites()
    {
        WeaponSprites weaponSprites = new WeaponSprites();
        var path = Path.Combine(Application.persistentDataPath, _weaponSpriteFileName);

        if (!File.Exists(path))
        {
            return;
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            weaponSprites = JsonUtility.FromJson<WeaponSprites>(loadJson);

            if (weaponSprites != null)
            {
                GameManager.Instance.CurrentWeaponSpriteList = weaponSprites.CurrentWeaponSpriteList;
            }
        }
    }

    public void JsonSaveWeaponSprites()
    {
        var path = Path.Combine(Application.persistentDataPath, _weaponSpriteFileName);

        WeaponSprites weaponSprites = new WeaponSprites();
        weaponSprites.CurrentWeaponSpriteList = GameManager.Instance.CurrentWeaponSpriteList;

        string json = JsonUtility.ToJson(weaponSprites, true);
        File.WriteAllText(path, json);
    }

    public void JsonClear() // Clear Data
    {
        var path = Path.Combine(Application.persistentDataPath, _dbFileName);

        SaveData saveData = new SaveData();
        saveData.CompletedMap = 0;
        saveData.TotalCoin = 0;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

}
