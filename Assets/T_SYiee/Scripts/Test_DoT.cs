using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //importing

public class Test_DoT : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    //public Renderer renderer;
    public SpriteRenderer image;

    //void Start()
    //{
    //    // ���� ���� �����ϴ� Tween �۾� ����
    //    //Tween colorTween = DOTween.To(() => startColor, color => renderer.material.color = color, endColor, 5f);

    //    // Tween �۾� ����
    //    //colorTween.Play().SetDelay(3f);
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            image.DOFade(0.2f, 0.5f).SetLoops(6, LoopType.Yoyo);
            //image.DOFade(0.5f, 1f);
            //image.DOColor(endColor, 0.5f).Loops();
        }
    }
}