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
            drawnTexture = null; // �׸� �ʱ�ȭ
        }
    }

    void SaveAndApplyWeapon()
    {
        if (drawnTexture != null)
        {
            // ����� �׸��� ������ �ؽ�ó�� ����
            Material weaponMaterial = weapon.GetComponent<Renderer>().material;
            weaponMaterial.mainTexture = drawnTexture;
        }
        //else
        //{
        //    // �׸��� ���� ��� �⺻ ���� �ؽ�ó�� ����
        //    Material weaponMaterial = weapon.GetComponent<Renderer>().material;
        //    weaponMaterial.mainTexture = defaultWeaponTexture;
        //}
        Debug.Log("Save");
    }
}
