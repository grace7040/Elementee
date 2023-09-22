using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Colors color;
    M_ColorState colorState;
    private void Awake()
    {
        SetColor();
    }
    void SetColor()
    {
        switch (color)
        {
            case Colors.def:
                colorState = new M_DefaultColor();
                break;

            case Colors.red:
                colorState = new M_RedColor();
                break;
        }
    }
}
