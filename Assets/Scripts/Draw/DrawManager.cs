using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    Colors color;


    // � Mode����
    public bool face_mode;

    public TMP_Text text;
    public SpriteRenderer testRenderer;

    public GameObject Drawing;
    public GameObject DrawbleObject;

    [Header("Camera")]
    public GameObject Cam;
    public GameObject DrawCam;


    [Header("Sprites")]
    public GameObject BackGround;
    public Sprite[] sprites;
    public Sprite[] Basic_Sprites;
    public Sprite[] BG_Basic_Sprites;
    public Sprite[] BG_Sprites;
    public List<string> DrawText;


    [Header("PenWidth")]
    public int weaponWidth = 10;
    public int faceWidth = 22;

    [HideInInspector]
    public DrawingSettings DrawSetting;


    string weaponDir;
    string soundDir;

    private static DrawManager instance = null;
    public static DrawManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Cam = GameObject.Find("Camera");
        DrawSetting = GetComponentInChildren<DrawingSettings>();
    }



    public void SetBrushColor(Colors color)
    {
        this.color = color;
        Color c = ColorManager.Instance.GetColor(color);

        BackGround.GetComponent<Image>().sprite = BG_Sprites[(int)color];
        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(weaponWidth);
        DrawSetting.SetMarkerColour(c);
    }

    public void OpenDrawing()
    {
        // Draw�� ī�޶��
        Cam.SetActive(false);
        DrawCam.SetActive(true);

        Drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        // �ٽ� ���� ī�޶��
        Cam.SetActive(true);
        DrawCam.SetActive(false);
        Drawing.SetActive(false);
    }


    // �� �׸���
    public void SetFaceColor()
    {
        Color c = ColorManager.Instance.GetColor(Colors.black);

        BackGround.GetComponent<Image>().sprite = BG_Sprites[8];

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = sprites[8];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(faceWidth);

        DrawSetting.SetMarkerColour(c);

    }

    public void SaveFaceDrawing()
    {
        // �ٽ� ���� ī�޶��
        Cam.SetActive(true);
        DrawCam.SetActive(false);

        Drawing.SetActive(false);
       // Managers.UI.ShowPopupUI<UI_Custom>();

        // Face ���� - 8��
        GameManager.Instance.playerFace = sprites[8];
    }


    // ĵ���� �⺻ ���� on off
    public void BasicWeapons(int num)
    {
        if(num==1)
            BackGround.GetComponent<Image>().sprite = BG_Basic_Sprites[(int)color];
        else
            BackGround.GetComponent<Image>().sprite = BG_Sprites[(int)color];
    }


    public void LoadWeapons(string _weaponDir)
    {
       // text.text = "����� Weapon �ҷ�����. weaponDir: " + _weaponDir;
        weaponDir = _weaponDir;

        for (int i = 0; i < sprites.Length; i++)
        {
            Texture2D texture= new Texture2D(0, 0);

            string filename = weaponDir + "/" + sprites[i].name + ".png";

           // text.text = i+"��° ����: "+ filename;

            byte[] byteTexture = File.ReadAllBytes(Application.persistentDataPath + filename);

           // text.text = i+"��° ����Ʈ �ؽ�ó Read �Ϸ�";

            if (byteTexture.Length > 0)
            {
                texture.LoadImage(byteTexture);
          //      text.text = i+"��° ����Ʈ �ؽ�ó�� �ؽ�ó�� �ε� �Ϸ�";
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
          //  text.text = i+"��° ��������Ʈ ���� �Ϸ�";
            sprite.name = sprites[i].name;
            sprites[i] = sprite;
          //  text.text = i+"��° ��������Ʈ ���� �Ϸ�";
        }

      //  text.text = "DrawManager.LoadWeapon �Ϸ�";
    }

    public void SaveWeapon(int i)
    {
      //  text.text = "SaveWeapon ����";
        byte[] bytes = sprites[i].texture.EncodeToPNG();
     //   text.text = i + "��° �ؽ�ó�� ����Ʈ�� ��ȯ �Ϸ�";
        string filename = weaponDir + "/" + sprites[i].name + ".png";
        File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
      //  text.text = i + "��° Weapon�ؽ�ó�� ���� �Ϸ�";
      //  testRenderer.sprite = sprites[i];
    }

    public void SaveWeapons(string _weaponDir)
    {
     //   text.text = "SaveWeapons ����";
        for (int i = 0; i < sprites.Length; i++)
        {
            byte[] bytes = sprites[i].texture.EncodeToPNG();
          //  text.text = i + "��° �ؽ�ó�� ����Ʈ�� ��ȯ �Ϸ�";
            string filename = _weaponDir + "/" + sprites[i].name + ".png";
            File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
          //  text.text = i + "��° Weapon�ؽ�ó�� ���� �Ϸ�";
        }
      //  text.text = "DrawManager.SaveWeapons �Ϸ�";
        Debug.Log(Application.persistentDataPath + weaponDir + "/" + sprites[0].name);
    }
}
