using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController player;

    public void SetColorState(IColorState _color)
    {
        player = FindObjectOfType<PlayerController>();
        player.Color = _color;
    }
}
