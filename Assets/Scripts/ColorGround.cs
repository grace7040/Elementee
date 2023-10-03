using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGround : MonoBehaviour
{
    public Colors colors;

    IColorState groundColor;

    private void Start()
    {
        SetGroundColor();
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
                groundColor = new DefaultColor();
                break;

            case Colors.blue:
                groundColor = new RedColor();
                break;

            case Colors.orange:
                groundColor = new DefaultColor();
                break;

            case Colors.green:
                groundColor = new RedColor();
                break;

            case Colors.purple:
                groundColor = new DefaultColor();
                break;

            case Colors.black:
                groundColor = new RedColor();
                break;
        }
    }
}
