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
        text.text = "���ϰ����� Start";
        CheckWeaponDir();
    }
    

    void CheckWeaponDir()
    {
        text.text = "CheckWeaponDir";
        string dir = Application.persistentDataPath + weaponDir;
        if (Directory.Exists(dir))
        {
            text.text = "WeaponDir ������";
            return;
        }

        text.text = "WeaponDir �������� ����";

        Directory.CreateDirectory(dir);

        text.text = "WeaponDir ���� �Ϸ�. ��������Ʈ ���� ����";

        for (int i = 0; i<drawManager.sprites.Length; i++)
        {
            string filename = "/" + drawManager.sprites[i].name;
            File.Create(dir + filename);
            text.text = "��������Ʈ ����: "+ dir + filename;
        }
        text.text = "��������Ʈ ���� �Ϸ�";
        drawManager.SaveWeapons(weaponDir);
    }

}
