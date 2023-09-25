using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRed : MonoBehaviour
{
    RedColor red;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColorManager.Instance.SetColorState(red);
        //ColorManager.Instance.SetColorState(new RedColor());
    }
}
