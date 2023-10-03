using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    BoxCollider2D boxcollider;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ColorManager.Instance.SetColorState(Colors.def);
        }
    }

}
