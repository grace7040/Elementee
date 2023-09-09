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
            // ����ڰ� �׸� �׸��� ���� ��� �Է��� ó���Ͽ� �׸��� �׸��ϴ�.
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
        // �׸� �׸��⸦ �����մϴ�.
        isDrawing = true;

        // �׸��� �׸� Texture2D�� �����մϴ�.
        drawingTexture = new Texture2D((int)drawingArea.rectTransform.rect.width, (int)drawingArea.rectTransform.rect.height);

        CreateNewLine();

        Debug.Log(1);
    }

    void ContinueDrawing()
    {
        // ����ڰ� �׸��� �׸��� ���� ��� ȣ��˴ϴ�.
        // ���⿡�� ���콺 ��ġ�� ���� �׷������ϴ�.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mousePosition, points[points.Count - 1]) > 0.1f)
        {
            points.Add(mousePosition);
            currentLine.positionCount = points.Count;
            List<Vector3> points3D = new List<Vector3>();

            foreach (Vector2 point in points)
            {
                Vector3 point3D = new Vector3(point.x, point.y, 0f); // Vector2�� Vector3�� ��ȯ
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
        // �׸� �׸��⸦ �����ϰ� �׸��� Texture2D�� �����մϴ�.
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
            Vector3 point3D = new Vector3(point.x, point.y, 0f); // Vector2�� Vector3�� ��ȯ
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
