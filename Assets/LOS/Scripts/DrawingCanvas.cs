using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{
    public RawImage drawingArea;
    private Texture2D drawingTexture;

    private bool isDrawing = true;

    private LineRenderer currentLine;
    private List<Vector2> points = new List<Vector2>();
    public GameObject linePrefab;

    void Update()
    {
        if (isDrawing)
        {
            // 사용자가 그림 그리기 중인 경우 입력을 처리하여 그림을 그립니다.
            if (Input.GetMouseButtonDown(0))
            {
                isDrawing = true;
                StartDrawing();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopDrawing();
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueDrawing();
            }
        }
    }

    void StartDrawing()
    {
        // 그림 그리기를 시작합니다.
        isDrawing = true;

        // 그림을 그릴 Texture2D를 생성합니다.
        drawingTexture = new Texture2D((int)drawingArea.rectTransform.rect.width, (int)drawingArea.rectTransform.rect.height);

        CreateNewLine();

        Debug.Log(1);
    }

    void ContinueDrawing()
    {
        // 사용자가 그림을 그리는 동안 계속 호출됩니다.
        // 여기에서 마우스 위치에 선을 그려나갑니다.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mousePosition, points[points.Count - 1]) > 0.1f)
        {
            points.Add(mousePosition);
            currentLine.positionCount = points.Count;
            List<Vector3> points3D = new List<Vector3>();

            foreach (Vector2 point in points)
            {
                Vector3 point3D = new Vector3(point.x, point.y, 0f); // Vector2를 Vector3로 변환
                points3D.Add(point3D);
            }

            currentLine.positionCount = points3D.Count;
            currentLine.SetPositions(points3D.ToArray());
            //currentLine.SetPositions(points.ToArray());
        }
        Debug.Log(2);
    }

    void StopDrawing()
    {
        // 그림 그리기를 중지하고 그림을 Texture2D에 저장합니다.
        isDrawing = false;
        drawingTexture.Apply();
        Debug.Log(3);
    }
    void CreateNewLine()
    {
        GameObject newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine = newLine.GetComponent<LineRenderer>();
        points.Clear();
        points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentLine.positionCount = 1;

        List<Vector3> points3D = new List<Vector3>();

        foreach (Vector2 point in points)
        {
            Vector3 point3D = new Vector3(point.x, point.y, 0f); // Vector2를 Vector3로 변환
            points3D.Add(point3D);
        }

        currentLine.positionCount = points3D.Count;
        currentLine.SetPositions(points3D.ToArray());

        //currentLine.SetPositions(points.ToArray());
    }

    public Texture2D GetDrawnTexture()
    {
        return drawingTexture;
    }
}
