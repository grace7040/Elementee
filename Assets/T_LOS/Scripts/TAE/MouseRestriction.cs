using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseRestriction : MonoBehaviour
{
    public RectTransform canvasRectTransform; // 캔버스의 RectTransform 참조

    private void Update()
    {
        // 현재 마우스 위치를 스크린 좌표로 가져옵니다.
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 좌표를 캔버스 영역 내에서의 좌표로 변환합니다.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, mousePosition, null, out Vector2 canvasMousePosition);

        // 캔버스 영역의 크기를 가져옵니다.
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // 마우스 좌표를 캔버스 영역 내에서 클래핑(제한)합니다.
        canvasMousePosition.x = Mathf.Clamp(canvasMousePosition.x, -canvasSize.x / 2, canvasSize.x / 2);
        canvasMousePosition.y = Mathf.Clamp(canvasMousePosition.y, -canvasSize.y / 2, canvasSize.y / 2);

        // 제한된 좌표를 다시 월드 좌표로 변환합니다.
        Vector3 worldMousePosition = canvasRectTransform.TransformPoint(canvasMousePosition);

        // 마우스 커서 위치를 제한된 위치로 이동시킵니다.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true; // 커서를 보이게 설정

        // 제한된 위치로 마우스 커서를 이동시킵니다.
        Vector3 clampedMousePosition = new Vector3(worldMousePosition.x, worldMousePosition.y, mousePosition.z);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 커서 모양을 기본값으로 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (Input.mousePosition != clampedMousePosition)
        {
            // 마우스 위치가 제한된 위치와 다를 때만 마우스를 이동시킵니다.
            Cursor.visible = false; // 커서를 숨깁니다.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
