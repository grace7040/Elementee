using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    Colors _color;
    
    //public TMP_Text text;

    public GameObject DrawbleObject;
    [SerializeField] GameObject _drawing;

    public bool IsDrawingFace;

    [Header("Camera")]
    [SerializeField] GameObject _drawCam;
    GameObject Cam;


    [Header("Sprites")]
    public Sprite[] WeaponCanvas;
    public Sprite[] BasicWeapon;
    public List<string> DrawText;
    [SerializeField] GameObject _sketch;
    [SerializeField] Sprite[] _basicWeaponBG;
    [SerializeField] Sprite[] _weaponCanvasBG;


    [Header("PenWidth")]
    int _weaponWidth = 30;
    int _faceWidth = 40;

    DrawingSettings DrawSetting;

    string _weaponDir;
    string _soundDir;

    static DrawManager _instance = null;
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
        LoadAllWeaponsFromDevice();
    }



    public void SetBrushColor(Colors color)
    {
        _color = color;
        Color c = ColorManager.Instance.GetColor(color);

        _sketch.GetComponent<Image>().sprite = _weaponCanvasBG[(int)color];
        DrawbleObject.GetComponent<SpriteRenderer>().sprite = WeaponCanvas[(int)color];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(_weaponWidth);
        DrawSetting.SetMarkerColour(c);
    }

    public void OpenDrawing()
    {
        Cam.SetActive(false);
        _drawCam.SetActive(true);
        _drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        Cam.SetActive(true);
        _drawCam.SetActive(false);
        _drawing.SetActive(false);
    }


    public void SetFaceColor()
    {
        IsDrawingFace = true;
        Color c = ColorManager.Instance.GetColor(Colors.Black);

        _sketch.GetComponent<Image>().sprite = _weaponCanvasBG[8];

        DrawbleObject.GetComponent<SpriteRenderer>().sprite = WeaponCanvas[8];
        DrawbleObject.GetComponent<Drawable>().UpdateCanvas();
        DrawSetting.SetMarkerWidth(_faceWidth);

        DrawSetting.SetMarkerColour(c);

    }

    public void SaveFaceDrawingOnDevice()
    {

        CloseDrawing();

        // Face 저장 - 8번
        GameManager.Instance.PlayerFace = WeaponCanvas[8];
        SaveWeaponOnDevice(8);

    }


    // 캔버스 기본 무기 on off
    public void UseBasicWeapons(bool value)
    {
        if (value)
            _sketch.GetComponent<Image>().sprite = _basicWeaponBG[(int)_color];
        else
            _sketch.GetComponent<Image>().sprite = _weaponCanvasBG[(int)_color];
    }


    public void LoadAllWeaponsFromDevice()
    {
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

    public void SaveWeaponOnDevice(int i)
    {
        byte[] bytes = WeaponCanvas[i].texture.EncodeToPNG();
        string filename = _weaponDir + "/" + WeaponCanvas[i].name + ".png";
        File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
    }

    public void SaveAllWeaponsOnDevice(string _weaponDir)
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