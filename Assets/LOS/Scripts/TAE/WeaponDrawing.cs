using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDrawing : MonoBehaviour
{
    //public GameObject playerWeapon; // 플레이어 무기 GameObject에 대한 참조
    //public Texture2D defaultWeaponTexture; // 기본 무기 텍스처
    //public RectTransform canvasRectTransform; // 캔버스 UI의 RectTransform 참조
    //public RawImage canvasImage; // 무기 그림을 그릴 캔버스 이미지
    //public Button finishButton; // 그림 완료 버튼
    //public float brushSize = 10f; // 브러시 크기
    //public Color brushColor = Color.black; // 브러시 색상

    //private Texture2D canvasTexture; // 그림을 그릴 캔버스 텍스처
    //private Material weaponMaterial; // 무기 메시에 적용될 머티리얼

    //private bool isDrawing;
    //private Vector2 lastDrawPosition;
    //private bool isDrawingEnabled;

    //private void Start()
    //{
    //    // 캔버스 텍스처 초기화
    //    canvasTexture = new Texture2D((int)canvasRectTransform.sizeDelta.x, (int)canvasRectTransform.sizeDelta.y);
    //    canvasTexture.filterMode = FilterMode.Point;

    //    // 무기 메시 머티리얼 초기화
    //    weaponMaterial = playerWeapon.GetComponent<MeshRenderer>().material;

    //    // 캔버스 이미지 초기화
    //    canvasImage.texture = canvasTexture;

    //    // 기본 무기 텍스처 설정
    //    SetWeaponTexture(defaultWeaponTexture);

    //    // '완료' 버튼의 클릭 이벤트 리스너 추가
    //    finishButton.onClick.AddListener(OnFinishButtonClick);
    //}

    //private void Update()
    //{
    //    // 'T' 키를 누르면 그리기 모드를 활성화/비활성화
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        isDrawingEnabled = !isDrawingEnabled;

    //        // 그리기 모드가 활성화되면 캔버스 이미지 활성화
    //        canvasImage.enabled = isDrawingEnabled;

    //        // 그리기 모드가 비활성화되면 그림을 무기로 적용하고 캔버스 초기화
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
    //        // 현재 마우스 위치에서 이전 위치까지 선 그리기
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
    //    // 그린 그림을 무기로 적용
    //    SetWeaponTexture(canvasTexture);
    //}

    //private void SetWeaponTexture(Texture2D texture)
    //{
    //    weaponMaterial.mainTexture = texture;
    //}

    public GameObject playerWeapon; // 플레이어 무기 GameObject에 대한 참조
    public LineRenderer lineRenderer; // 그림을 그리기 위한 Line Renderer
    public RectTransform canvasRectTransform; // 캔버스의 RectTransform 참조
    public KeyCode drawKey = KeyCode.T; // 그림 그리기를 시작 및 종료할 키

    private Mesh weaponMesh; // 무기 Mesh에 대한 참조
    private bool isDrawing = false; // 그림 그리는 중인지 여부

    private void Start()
    {
        // 무기 Mesh를 가져와서 복사합니다.
        weaponMesh = Instantiate(playerWeapon.GetComponent<MeshFilter>().sharedMesh);

        // 캔버스 UI를 초기에 비활성화합니다.
        DisableCanvasUI();
    }

    private void Update()
    {
        // 'T' 키를 눌렀을 때 캔버스 UI를 토글합니다.
        if (Input.GetKeyDown(drawKey))
        {
            ToggleCanvasUI();
        }

        // 그림 그리기 중일 때만 Line Renderer를 업데이트합니다.
        if (isDrawing)
        {
            UpdateLineRenderer();
        }
    }

    private void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.enabled = true;

        // 새로 그리기 시작할 때 이전 그림을 지웁니다.
        lineRenderer.positionCount = 0;
    }

    private void StopDrawing()
    {
        isDrawing = false;
        lineRenderer.enabled = false;
    }

    private void UpdateLineRenderer()
    {
        // 마우스 위치를 캔버스 영역 내에서의 좌표로 변환합니다.
        Vector3 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform, mousePosition, Camera.main, out Vector3 worldMousePosition);

        // Line Renderer의 점을 업데이트합니다.
        int positionCount = lineRenderer.positionCount;
        lineRenderer.positionCount = positionCount + 1;
        lineRenderer.SetPosition(positionCount, worldMousePosition);
    }

    private void ToggleCanvasUI()
    {
        // 캔버스 UI를 토글합니다.
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
        // 캔버스 UI를 활성화하고 그림 그리기를 시작합니다.
        StartDrawing();
        canvasRectTransform.gameObject.SetActive(true);
    }

    private void DisableCanvasUI()
    {
        // 캔버스 UI를 비활성화하고 그림 그리기를 종료합니다.
        isDrawing = false;
        lineRenderer.enabled = false;
        canvasRectTransform.gameObject.SetActive(true);
    }

    private void ApplyDrawingToWeaponMesh()
    {
        // Line Renderer를 사용하여 그린 그림을 Mesh로 변환합니다.
        Mesh drawingMesh = new Mesh();
        lineRenderer.BakeMesh(drawingMesh);

        // 무기 Mesh를 그린 그림 Mesh로 교체합니다.
        playerWeapon.GetComponent<MeshFilter>().mesh = drawingMesh;
    }
}
