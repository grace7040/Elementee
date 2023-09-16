using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public GameObject drawingCanvas;
    public Button finishButton;

    private bool isDrawing = false;
    private Texture2D drawnTexture;

    public GameObject weapon;

    void Start()
    {
        drawingCanvas.SetActive(false);
        finishButton.onClick.AddListener(SaveAndApplyWeapon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleDrawingUI();
        }
    }

    void ToggleDrawingUI()
    {
        isDrawing = !isDrawing;
        drawingCanvas.SetActive(isDrawing);
        if (!isDrawing)
        {
            drawnTexture = null; // 그림 초기화
        }
    }

    void SaveAndApplyWeapon()
    {
        if (drawnTexture != null)
        {
            // 저장된 그림을 무기의 텍스처로 설정
            Material weaponMaterial = weapon.GetComponent<Renderer>().material;
            weaponMaterial.mainTexture = drawnTexture;
        }
        //else
        //{
        //    // 그림이 없을 경우 기본 무기 텍스처로 설정
        //    Material weaponMaterial = weapon.GetComponent<Renderer>().material;
        //    weaponMaterial.mainTexture = defaultWeaponTexture;
        //}
        Debug.Log("Save");
    }
}
