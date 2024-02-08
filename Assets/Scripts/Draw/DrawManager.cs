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


    // 어떤 Mode인지
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
        // Draw용 카메라로
        Cam.SetActive(false);
        DrawCam.SetActive(true);

        Drawing.SetActive(true);
    }

    public void CloseDrawing()
    {
        // 다시 원래 카메라로
        Cam.SetActive(true);
        DrawCam.SetActive(false);
        Drawing.SetActive(false);
    }


    // 얼굴 그리기
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
        // 다시 원래 카메라로
        Cam.SetActive(true);
        DrawCam.SetActive(false);

        Drawing.SetActive(false);
       // Managers.UI.ShowPopupUI<UI_Custom>();

        // Face 저장 - 8번
        GameManager.Instance.playerFace = sprites[8];
    }


    // 캔버스 기본 무기 on off
    public void BasicWeapons(int num)
    {
        if(num==1)
            BackGround.GetComponent<Image>().sprite = BG_Basic_Sprites[(int)color];
        else
            BackGround.GetComponent<Image>().sprite = BG_Sprites[(int)color];
    }


    public void LoadWeapons(string _weaponDir)
    {
       // text.text = "저장된 Weapon 불러오기. weaponDir: " + _weaponDir;
        weaponDir = _weaponDir;

        for (int i = 0; i < sprites.Length; i++)
        {
            Texture2D texture= new Texture2D(0, 0);

            string filename = weaponDir + "/" + sprites[i].name + ".png";

           // text.text = i+"번째 파일: "+ filename;

            byte[] byteTexture = File.ReadAllBytes(Application.persistentDataPath + filename);

           // text.text = i+"번째 바이트 텍스처 Read 완료";

            if (byteTexture.Length > 0)
            {
                texture.LoadImage(byteTexture);
          //      text.text = i+"번째 바이트 텍스처를 텍스처로 로드 완료";
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
          //  text.text = i+"번째 스프라이트 생성 완료";
            sprite.name = sprites[i].name;
            sprites[i] = sprite;
          //  text.text = i+"번째 스프라이트 적용 완료";
        }

      //  text.text = "DrawManager.LoadWeapon 완료";
    }

    public void SaveWeapon(int i)
    {
      //  text.text = "SaveWeapon 시작";
        byte[] bytes = sprites[i].texture.EncodeToPNG();
     //   text.text = i + "번째 텍스처를 바이트로 변환 완료";
        string filename = weaponDir + "/" + sprites[i].name + ".png";
        File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
      //  text.text = i + "번째 Weapon텍스처를 저장 완료";
      //  testRenderer.sprite = sprites[i];
    }

    public void SaveWeapons(string _weaponDir)
    {
     //   text.text = "SaveWeapons 시작";
        for (int i = 0; i < sprites.Length; i++)
        {
            byte[] bytes = sprites[i].texture.EncodeToPNG();
          //  text.text = i + "번째 텍스처를 바이트로 변환 완료";
            string filename = _weaponDir + "/" + sprites[i].name + ".png";
            File.WriteAllBytes(Application.persistentDataPath + filename, bytes);
          //  text.text = i + "번째 Weapon텍스처를 저장 완료";
        }
      //  text.text = "DrawManager.SaveWeapons 완료";
        Debug.Log(Application.persistentDataPath + weaponDir + "/" + sprites[0].name);
    }
}
