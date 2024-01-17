using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseRestriction : MonoBehaviour
{
    public RectTransform canvasRectTransform; // ĵ������ RectTransform ����

    private void Update()
    {
        // ���� ���콺 ��ġ�� ��ũ�� ��ǥ�� �����ɴϴ�.
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 ��ǥ�� ĵ���� ���� �������� ��ǥ�� ��ȯ�մϴ�.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, mousePosition, null, out Vector2 canvasMousePosition);

        // ĵ���� ������ ũ�⸦ �����ɴϴ�.
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // ���콺 ��ǥ�� ĵ���� ���� ������ Ŭ����(����)�մϴ�.
        canvasMousePosition.x = Mathf.Clamp(canvasMousePosition.x, -canvasSize.x / 2, canvasSize.x / 2);
        canvasMousePosition.y = Mathf.Clamp(canvasMousePosition.y, -canvasSize.y / 2, canvasSize.y / 2);

        // ���ѵ� ��ǥ�� �ٽ� ���� ��ǥ�� ��ȯ�մϴ�.
        Vector3 worldMousePosition = canvasRectTransform.TransformPoint(canvasMousePosition);

        // ���콺 Ŀ�� ��ġ�� ���ѵ� ��ġ�� �̵���ŵ�ϴ�.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true; // Ŀ���� ���̰� ����

        // ���ѵ� ��ġ�� ���콺 Ŀ���� �̵���ŵ�ϴ�.
        Vector3 clampedMousePosition = new Vector3(worldMousePosition.x, worldMousePosition.y, mousePosition.z);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Ŀ�� ����� �⺻������ ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (Input.mousePosition != clampedMousePosition)
        {
            // ���콺 ��ġ�� ���ѵ� ��ġ�� �ٸ� ���� ���콺�� �̵���ŵ�ϴ�.
            Cursor.visible = false; // Ŀ���� ����ϴ�.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
