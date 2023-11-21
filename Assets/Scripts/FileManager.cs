using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public DrawManager drawManager;
    public TMP_Text text;

    string weaponDir = "/weapons";
    void Start()
    {
        text.text = "파일관리자 Start";
        CheckWeaponDir();
    }
    

    void CheckWeaponDir()
    {
        text.text = "CheckWeaponDir";
        string dir = Application.persistentDataPath + weaponDir;
        if (Directory.Exists(dir))
        {
            text.text = "WeaponDir 존재함";
            return;
        }

        text.text = "WeaponDir 존재하지 않음";

        Directory.CreateDirectory(dir);

        text.text = "WeaponDir 생성 완료. 스프라이트 저장 시작";

        for (int i = 0; i<drawManager.sprites.Length; i++)
        {
            string filename = "/" + drawManager.sprites[i].name;
            File.Create(dir + filename);
            text.text = "스프라이트 저장: "+ dir + filename;
        }
        text.text = "스프라이트 저장 완료";
        drawManager.SaveWeapons(weaponDir);
    }

}
