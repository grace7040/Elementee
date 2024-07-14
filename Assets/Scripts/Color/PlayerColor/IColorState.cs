using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorState
{
    public int Damage { get; }
    public bool WallSliding { get; }
    public float CoolTime { get; }
    public void Attack(Vector3 playerPosition, float playerLocalScaleX);

}

