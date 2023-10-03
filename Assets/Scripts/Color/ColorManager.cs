using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController player;

    [Header("Color State")]
    public bool hasRed = false;
    public bool hasBlue = false;
    public bool hasYellow = false;

    public void SetColorState(IColorState _color)
    {
        player = FindObjectOfType<PlayerController>();
        player.Color = _color;
    }
}
