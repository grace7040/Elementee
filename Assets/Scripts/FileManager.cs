using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public DrawManager DrawManager;
    public TMP_Text Text;

    string weaponDir = "/weapons";
    void Start()
    {
        Text.text = "파일관리자 Start";
        CheckWeaponDir();
    }
    

    void CheckWeaponDir()
    {
        Text.text = "CheckWeaponDir";
        string dir = Application.persistentDataPath + weaponDir;
        if (Directory.Exists(dir))
        {
            Text.text = "WeaponDir 존재함";
            return;
        }

        Text.text = "WeaponDir 존재하지 않음";

        Directory.CreateDirectory(dir);

        Text.text = "WeaponDir 생성 완료. 스프라이트 저장 시작";

        for (int i = 0; i<DrawManager.WeaponCanvas.Length; i++)
        {
            string filename = "/" + DrawManager.WeaponCanvas[i].name;
            File.Create(dir + filename);
            Text.text = "스프라이트 저장: "+ dir + filename;
        }
        Text.text = "스프라이트 저장 완료";
        DrawManager.SaveWeapons(weaponDir);
    }

}
