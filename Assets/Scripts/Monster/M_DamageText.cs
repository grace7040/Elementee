using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class M_DamageText : PoolAble
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(13, 1.3f);
        this.gameObject.GetComponent<TextMeshPro>().DOFade(0,2f);
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
