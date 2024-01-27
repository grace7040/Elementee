using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class FootBoard_wEndPoint : MonoBehaviour
{
    public Transform endPoint;
    [SerializeField] private float time = 3;

    public enum Mode { Linear, InOutQuad };
    public Mode mode = Mode.Linear;

    

    private void Start()
    {
        if(mode == Mode.Linear)
            transform.DOMove(endPoint.position, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        else
            transform.DOMove(endPoint.position, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(this.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(null);

    }

}
