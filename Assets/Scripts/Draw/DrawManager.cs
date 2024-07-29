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

    public TMP_Text text;
    public SpriteRenderer testRenderer;

    public GameObject Drawing;
    public GameObject DrawbleObject;

    public bool face_mode;

    [Header("Camera")]
    public GameObject Cam;
    public GameObject DrawCam;


    [Header("Sprites")]
    public GameObject BackGround;
    public Sprite[] WeaponCanvas;
    public Sprite[] BasicWeapon;
    public Sprite[] BasicWeaponBG;
    public Sprite[] WeaponCanvasBG;
    public List<string> DrawText;


    [Header("PenWidth")]
    public int WeaponWidth = 10;
    public int FaceWidth = 22;


    [HideInInspector]
    public DrawingSettings DrawSetting;


    string _weaponDir;
    string _soundDir;

    private static DrawManager _instance = null;
    public static DrawManager Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
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

        BackGround.GetComponent<Image>().sprite = WeaponCanvasBG[(int)color];
        DrawbleObject.GetComponent<SpriteRenderer>().sprite = WeaponCanvas[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(WeaponWidth);
        DrawSetting.SetMarkerColour(c);
    }

    public void OpenDrawing()
    {
        Cam.SetActive(false);
        DrawCam.SetActive(true);
        Drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        Cam.SetActive(true);
        DrawCam.SetActive(false);
        Drawing.SetActive(false);
    }


    public void SetFaceColor()
    {
        face_mode = true;
        Color c = ColorManager.Instance.GetColor(Colors.Black);

        BackGround.GetComponent<Image>().sprite = WeaponCanvasBG[8];

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = WeaponCanvas[8];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(FaceWidth);

        DrawSetting.SetMarkerColour(c);

    }

    public void SaveFaceDrawing()
    {

        // 다시 원래 카메라로
        Cam.SetActive(true);
        DrawCam.SetActive(false);

        Drawing.SetActive(false);

        // Face 저장 - 8번
        GameManager.Instance.PlayerFace = WeaponCanvas[8];
        SaveWeapon(8);

    }


    // 캔버스 기본 무기 on off
    public void BasicWeapons(int num)
    {
        if (num == 1)
            BackGround.GetComponent<Image>().sprite = BasicWeaponBG[(int)color];
        else
            BackGround.GetComponent<Image>().sprite = WeaponCanvasBG[(int)color];
    }


    public void LoadWeapons(string _weaponDir)
    {

        this._weaponDir = _weaponDir;

        for (int i = 0; i < WeaponCanvas.Length; i++)
        {
            Texture2D texture = new Texture2D(0, 0);
            string filename = this._weaponDir + "/" + WeaponCanvas[i].name + ".png";
            byte[] byteTexture = File.ReadAllBytes(Application.persistentDataPath + filename);

            if (byteTexture.Length > 0)
            {
                texture.LoadImage(byteTexture);
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = WeaponCanvas[i].name;
            WeaponCanvas[i] = sprite;
        }

    }

    public void SaveWeapon(int i)
    {
        byte[] bytes = WeaponCanvas[i].texture.EncodeToPNG();
        string filename = _weaponDir + "/" + WeaponCanvas[i].name + ".png";
        File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
    }

    public void SaveWeapons(string _weaponDir)
    {
        for (int i = 0; i < WeaponCanvas.Length; i++)
        {
            byte[] bytes = WeaponCanvas[i].texture.EncodeToPNG();
            string filename = _weaponDir + "/" + WeaponCanvas[i].name + ".png";
            File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
        }
        Debug.Log(Application.persistentDataPath + this._weaponDir + "/" + WeaponCanvas[0].name);
    }
}