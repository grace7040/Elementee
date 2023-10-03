using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGround : MonoBehaviour
{
    public Colors colors;
    BoxCollider2D boxcollider;

    IColorState groundColor;

    public int force = 100;

    private void Start()
    {
        SetGroundColor();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    void SetGroundColor()
    {
        switch (colors)
        {
            case Colors.def:
                groundColor = new DefaultColor();
                break;

            case Colors.red:
                groundColor = new RedColor();
                break;

            case Colors.yellow:
                groundColor = new YellowColor();
                break;

            case Colors.blue:
                groundColor = new BlueColor();
                break;

            case Colors.orange:
                groundColor = new OrangeColor();
                break;

            case Colors.green:
                groundColor = new GreenColor();
                break;

            case Colors.purple:
                groundColor = new PurpleColor();
                break;

            case Colors.black:
                groundColor = new BlackColor();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().Color != groundColor)
        {
            //collision.gameObject.GetComponent<PlayerController>().TakeDamage(dam)
        }

    }

}
