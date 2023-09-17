using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRed : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColorManager.Instance.SetColorState(new RedColor());
    }
}
