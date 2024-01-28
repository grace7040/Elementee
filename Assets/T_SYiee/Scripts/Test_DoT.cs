using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //importing

public class Test_DoT : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public Renderer renderer;

    void Start()
    {
        // ���� ���� �����ϴ� Tween �۾� ����
        Tween colorTween = DOTween.To(() => startColor, color => renderer.material.color = color, endColor, 5f);

        // Tween �۾� ����
        colorTween.Play().SetDelay(3f);
    }
}