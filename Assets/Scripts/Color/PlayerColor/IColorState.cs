using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorState
{
    public float JumpForce { get; }
    public int Damage { get; }
    public bool WallSliding { get; }
    public float CoolTime { get; }
    public void Attack(PlayerController player);

}

