using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void SetColorState(ColorState _color)
    {
        player.Color = _color;
    }
}
