using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDrawing : MonoBehaviour
{
    //public GameObject playerWeapon; // �÷��̾� ���� GameObject�� ���� ����
    //public Texture2D defaultWeaponTexture; // �⺻ ���� �ؽ�ó
    //public RectTransform canvasRectTransform; // ĵ���� UI�� RectTransform ����
    //public RawImage canvasImage; // ���� �׸��� �׸� ĵ���� �̹���
    //public Button finishButton; // �׸� �Ϸ� ��ư
    //public float brushSize = 10f; // �귯�� ũ��
    //public Color brushColor = Color.black; // �귯�� ����

    //private Texture2D canvasTexture; // �׸��� �׸� ĵ���� �ؽ�ó
    //private Material weaponMaterial; // ���� �޽ÿ� ����� ��Ƽ����

    //private bool isDrawing;
    //private Vector2 lastDrawPosition;
    //private bool isDrawingEnabled;

    //private void Start()
    //{
    //    // ĵ���� �ؽ�ó �ʱ�ȭ
    //    canvasTexture = new Texture2D((int)canvasRectTransform.sizeDelta.x, (int)canvasRectTransform.sizeDelta.y);
    //    canvasTexture.filterMode = FilterMode.Point;

    //    // ���� �޽� ��Ƽ���� �ʱ�ȭ
    //    weaponMaterial = playerWeapon.GetComponent<MeshRenderer>().material;

    //    // ĵ���� �̹��� �ʱ�ȭ
    //    canvasImage.texture = canvasTexture;

    //    // �⺻ ���� �ؽ�ó ����
    //    SetWeaponTexture(defaultWeaponTexture);

    //    // '�Ϸ�' ��ư�� Ŭ�� �̺�Ʈ ������ �߰�
    //    finishButton.onClick.AddListener(OnFinishButtonClick);
    //}

    //private void Update()
    //{
    //    // 'T' Ű�� ������ �׸��� ��带 Ȱ��ȭ/��Ȱ��ȭ
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        isDrawingEnabled = !isDrawingEnabled;

    //        // �׸��� ��尡 Ȱ��ȭ�Ǹ� ĵ���� �̹��� Ȱ��ȭ
    //        canvasImage.enabled = isDrawingEnabled;

    //        // �׸��� ��尡 ��Ȱ��ȭ�Ǹ� �׸��� ����� �����ϰ� ĵ���� �ʱ�ȭ
    //        if (!isDrawingEnabled)
    //        {
    //            SetWeaponTexture(canvasTexture);
    //            canvasTexture.SetPixels(defaultWeaponTexture.GetPixels());
    //            canvasTexture.Apply();
    //        }
    //    }

    //    if (Input.GetMouseButton(0))
    //    {
    //        Debug.Log("1");
    //        // ���� ���콺 ��ġ���� ���� ��ġ���� �� �׸���
    //        Vector2 currentDrawPosition = Input.mousePosition;
    //        DrawLine(lastDrawPosition, currentDrawPosition, brushSize, brushColor);
    //        lastDrawPosition = currentDrawPosition;
    //    }
    //}

    //private void DrawLine(Vector2 startPos, Vector2 endPos, float size, Color color)
    //{
    //    Vector2 direction = (endPos - startPos).normalized;
    //    float distance = Vector2.Distance(startPos, endPos);

    //    for (float i = 0; i < distance; i += 0.1f)
    //    {
    //        Vector2 point = startPos + direction * i;
    //        DrawBrush(point, size, color);
    //    }
    //}

    //private void DrawBrush(Vector2 position, float size, Color color)
    //{
    //    int x = Mathf.RoundToInt(position.x);
    //    int y = Mathf.RoundToInt(position.y);

    //    for (int i = x - (int)size / 2; i < x + (int)size / 2; i++)
    //    {
    //        for (int j = y - (int)size / 2; j < y + (int)size / 2; j++)
    //        {
    //            if (i >= 0 && i < canvasTexture.width && j >= 0 && j < canvasTexture.height)
    //            {
    //                canvasTexture.SetPixel(i, j, color);
    //            }
    //        }
    //    }
    //    canvasTexture.Apply();
    //}

    //private void OnFinishButtonClick()
    //{
    //    // �׸� �׸��� ����� ����
    //    SetWeaponTexture(canvasTexture);
    //}

    //private void SetWeaponTexture(Texture2D texture)
    //{
    //    weaponMaterial.mainTexture = texture;
    //}

    public GameObject playerWeapon; // �÷��̾� ���� GameObject�� ���� ����
    public LineRenderer lineRenderer; // �׸��� �׸��� ���� Line Renderer
    public RectTransform canvasRectTransform; // ĵ������ RectTransform ����
    public KeyCode drawKey = KeyCode.T; // �׸� �׸��⸦ ���� �� ������ Ű

    private Mesh weaponMesh; // ���� Mesh�� ���� ����
    private bool isDrawing = false; // �׸� �׸��� ������ ����

    private void Start()
    {
        // ���� Mesh�� �����ͼ� �����մϴ�.
        weaponMesh = Instantiate(playerWeapon.GetComponent<MeshFilter>().sharedMesh);

        // ĵ���� UI�� �ʱ⿡ ��Ȱ��ȭ�մϴ�.
        DisableCanvasUI();
    }

    private void Update()
    {
        // 'T' Ű�� ������ �� ĵ���� UI�� ����մϴ�.
        if (Input.GetKeyDown(drawKey))
        {
            ToggleCanvasUI();
        }

        // �׸� �׸��� ���� ���� Line Renderer�� ������Ʈ�մϴ�.
        if (isDrawing)
        {
            UpdateLineRenderer();
        }
    }

    private void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.enabled = true;

        // ���� �׸��� ������ �� ���� �׸��� ����ϴ�.
        lineRenderer.positionCount = 0;
    }

    private void StopDrawing()
    {
        isDrawing = false;
        lineRenderer.enabled = false;
    }

    private void UpdateLineRenderer()
    {
        // ���콺 ��ġ�� ĵ���� ���� �������� ��ǥ�� ��ȯ�մϴ�.
        Vector3 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform, mousePosition, Camera.main, out Vector3 worldMousePosition);

        // Line Renderer�� ���� ������Ʈ�մϴ�.
        int positionCount = lineRenderer.positionCount;
        lineRenderer.positionCount = positionCount + 1;
        lineRenderer.SetPosition(positionCount, worldMousePosition);
    }

    private void ToggleCanvasUI()
    {
        // ĵ���� UI�� ����մϴ�.
        if (!isDrawing)
        {
            EnableCanvasUI();
        }
        else
        {
            ApplyDrawingToWeaponMesh();
            DisableCanvasUI();
        }
    }

    private void EnableCanvasUI()
    {
        // ĵ���� UI�� Ȱ��ȭ�ϰ� �׸� �׸��⸦ �����մϴ�.
        StartDrawing();
        canvasRectTransform.gameObject.SetActive(true);
    }

    private void DisableCanvasUI()
    {
        // ĵ���� UI�� ��Ȱ��ȭ�ϰ� �׸� �׸��⸦ �����մϴ�.
        isDrawing = false;
        lineRenderer.enabled = false;
        canvasRectTransform.gameObject.SetActive(true);
    }

    private void ApplyDrawingToWeaponMesh()
    {
        // Line Renderer�� ����Ͽ� �׸� �׸��� Mesh�� ��ȯ�մϴ�.
        Mesh drawingMesh = new Mesh();
        lineRenderer.BakeMesh(drawingMesh);

        // ���� Mesh�� �׸� �׸� Mesh�� ��ü�մϴ�.
        playerWeapon.GetComponent<MeshFilter>().mesh = drawingMesh;
    }
}
