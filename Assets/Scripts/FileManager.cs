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
        Text.text = "���ϰ����� Start";
        CheckWeaponDir();
    }
    

    void CheckWeaponDir()
    {
        Text.text = "CheckWeaponDir";
        string dir = Application.persistentDataPath + weaponDir;
        if (Directory.Exists(dir))
        {
            Text.text = "WeaponDir ������";
            return;
        }

        Text.text = "WeaponDir �������� ����";

        Directory.CreateDirectory(dir);

        Text.text = "WeaponDir ���� �Ϸ�. ��������Ʈ ���� ����";

        for (int i = 0; i<DrawManager.WeaponCanvas.Length; i++)
        {
            string filename = "/" + DrawManager.WeaponCanvas[i].name;
            File.Create(dir + filename);
            Text.text = "��������Ʈ ����: "+ dir + filename;
        }
        Text.text = "��������Ʈ ���� �Ϸ�";
        DrawManager.SaveWeapons(weaponDir);
    }

}
