using UnityEngine;
using UnityEngine.UI;

public class DrawingUI : MonoBehaviour
{
    //public Image drawingArea;
    //public GameObject playerWeapon;
    //public Button drawButton;
    //public Button eraseButton;
    //public Button finishButton;

    //private Texture2D canvasTexture;
    //private Color[] canvasColors;
    //private bool isDrawing = false;
    //private bool isErasing = false;
    //private RectTransform canvasRect;

    //void Start()
    //{
    //    canvasRect = drawingArea.GetComponent<RectTransform>();

    //    // �ʱ�ȭ �� �ؽ�ó ����
    //    int canvasWidth = (int)canvasRect.sizeDelta.x;
    //    int canvasHeight = (int)canvasRect.sizeDelta.y;
    //    canvasTexture = new Texture2D(canvasWidth, canvasHeight);
    //    canvasColors = new Color[canvasWidth * canvasHeight];

    //    // �⺻ �ؽ�ó �ʱ�ȭ (�������� ����)
    //    for (int i = 0; i < canvasColors.Length; i++)
    //    {
    //        canvasColors[i] = Color.clear;
    //    }

    //    canvasTexture.SetPixels(canvasColors);
    //    canvasTexture.Apply();
    //    drawingArea.sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasWidth, canvasHeight), Vector2.one * 0.5f);

    //    // ��ư Ŭ�� �̺�Ʈ ����
    //    drawButton.onClick.AddListener(EnableDrawing);
    //    eraseButton.onClick.AddListener(EnableErasing);
    //    finishButton.onClick.AddListener(FinishDrawing);
    //}

    //void Update()
    //{
    //    if ((isDrawing || isErasing) && Input.GetMouseButton(0))
    //    {
    //        Vector2 localPoint;
    //        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPoint))
    //        {
    //            int x = Mathf.FloorToInt(localPoint.x);
    //            int y = Mathf.FloorToInt(localPoint.y);

    //            if (x >= 0 && x < canvasTexture.width && y >= 0 && y < canvasTexture.height)
    //            {
    //                int index = y * canvasTexture.width + x;

    //                if (isDrawing)
    //                {
    //                    canvasColors[index] = Color.black; // �׸��� ���
    //                }
    //                else if (isErasing)
    //                {
    //                    canvasColors[index] = Color.clear; // ����� ���
    //                }

    //                canvasTexture.SetPixels(canvasColors);
    //                canvasTexture.Apply();
    //            }
    //        }
    //    }
    //}

    //void EnableDrawing()
    //{
    //    isDrawing = true;
    //    isErasing = false;
    //}

    //void EnableErasing()
    //{
    //    isDrawing = false;
    //    isErasing = true;
    //}

    //void FinishDrawing()
    //{
    //    isDrawing = false;
    //    isErasing = false;

    //    // ���� Mesh ũ�� ��� �� ����
    //    Bounds weaponBounds = playerWeapon.GetComponent<Renderer>().bounds;
    //    float canvasWidth = canvasRect.sizeDelta.x;
    //    float canvasHeight = canvasRect.sizeDelta.y;
    //    float weaponWidth = weaponBounds.size.x;
    //    float weaponHeight = weaponBounds.size.y;
    //    float scaleX = weaponWidth / canvasWidth;
    //    float scaleY = weaponHeight / canvasHeight;

    //    canvasRect.localScale = new Vector3(scaleX, scaleY, 1f);

    //    // Canvas �ؽ�ó�� ���� Mesh�� ����
    //    MeshRenderer weaponRenderer = playerWeapon.GetComponent<MeshRenderer>();
    //    weaponRenderer.material.mainTexture = canvasTexture;
    //}

    public Image drawingArea;
    public GameObject playerWeapon;
    public Button drawButton;
    public Button eraseButton;
    public Button finishButton;
    public LineRenderer lineRendererPrefab;

    private Texture2D canvasTexture;
    private Color[] canvasColors;
    private bool isDrawing = false;
    private bool isErasing = false;
    private RectTransform canvasRect;
    private LineRenderer currentLine;
    private int lineVertexCount = 0;

    void Start()
    {
        canvasRect = drawingArea.GetComponent<RectTransform>();

        // �ʱ�ȭ �� �ؽ�ó ����
        int canvasWidth = (int)canvasRect.sizeDelta.x;
        int canvasHeight = (int)canvasRect.sizeDelta.y;
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);
        canvasColors = new Color[canvasWidth * canvasHeight];

        // �⺻ �ؽ�ó �ʱ�ȭ (�������� ����)
        for (int i = 0; i < canvasColors.Length; i++)
        {
            canvasColors[i] = Color.white;
        }

        canvasTexture.SetPixels(canvasColors);
        canvasTexture.Apply();
        drawingArea.sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasWidth, canvasHeight), Vector2.one * 0.5f);

        // ��ư Ŭ�� �̺�Ʈ ����
        drawButton.onClick.AddListener(EnableDrawing);
        eraseButton.onClick.AddListener(EnableErasing);
        finishButton.onClick.AddListener(FinishDrawing);
    }

    void FixedUpdate()
    {
        if (isDrawing || isErasing)
        { 
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, new Vector2(Input.mousePosition.x + 220.3f, Input.mousePosition.y + 113.3f), null, out localPoint))
            {
                int x = Mathf.FloorToInt(localPoint.x);
                int y = Mathf.FloorToInt(localPoint.y);

                if (x >= 0 && x < canvasTexture.width && y >= 0 && y < canvasTexture.height)
                {
                    int index = y * canvasTexture.width + x;

                    if (isDrawing)
                    {
                        canvasColors[index] = Color.black; // �׸��� ���
                        UpdateLineRenderer(localPoint);
                    }
                    else if (isErasing)
                    {
                        canvasColors[index] = Color.white; // ����� ���
                       
                    }

                    canvasTexture.SetPixels(canvasColors);
                    canvasTexture.Apply();
                }
            }
        }
    }

    void EnableDrawing()
    {
        isDrawing = true;
        isErasing = false;
        CreateNewLineRenderer();
    }

    void EnableErasing()
    {
        isDrawing = false;
        isErasing = true;
    }

    void FinishDrawing()
    {
        isDrawing = false;
        isErasing = false;

        // ���� Mesh ũ�� ��� �� ����
        Bounds weaponBounds = playerWeapon.GetComponent<Renderer>().bounds;
        float canvasWidth = canvasRect.sizeDelta.x;
        float canvasHeight = canvasRect.sizeDelta.y;
        float weaponWidth = weaponBounds.size.x;
        float weaponHeight = weaponBounds.size.y;
        float scaleX = weaponWidth / canvasWidth;
        float scaleY = weaponHeight / canvasHeight;

        canvasRect.localScale = new Vector3(scaleX, scaleY, 1f);

        // Canvas �ؽ�ó�� ���� Mesh�� ����
        MeshRenderer weaponRenderer = playerWeapon.GetComponent<MeshRenderer>();
        weaponRenderer.material.mainTexture = canvasTexture;
    }

    void CreateNewLineRenderer()
    {
        currentLine = Instantiate(lineRendererPrefab, drawingArea.transform);
      //  currentLine = 
        currentLine.startWidth = 300f; // ���� �β��� �����մϴ�.
        currentLine.endWidth = 300f;   // �� �β��� �����մϴ�.
        currentLine.positionCount = 0;
        lineVertexCount = 0;
    }

    void UpdateLineRenderer(Vector3 point)
    {
        if (currentLine == null)
        {
            CreateNewLineRenderer();
        }

        // ���� ��ġ
        Vector3 prevPoint = currentLine.positionCount > 0 ? currentLine.GetPosition(currentLine.positionCount - 1) : point;

        // ���콺 �̵� ����
        Vector3 dir = point - prevPoint;
        float distance = dir.magnitude;

        // ���� �ε巴�� �׸��� ���� ���ø�
        int segments = Mathf.CeilToInt(distance / 0.01f); // 0.1f�� ���׸�Ʈ �����Դϴ�.

        // ���� ������ �߰�
        if (currentLine.positionCount == 0)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(lineVertexCount, point);
            lineVertexCount++;
        }

        for (int i = 0; i < segments; i++)
        {
            float t = (i + 1) / (float)segments; // �������� �̹� �߰������Ƿ� t�� 1/segments���� �����մϴ�.
            Vector3 newPosition = Vector3.Lerp(prevPoint, point, t);
            currentLine.positionCount++;
            currentLine.SetPosition(lineVertexCount, newPosition);
            lineVertexCount++;
        }
    }
}
