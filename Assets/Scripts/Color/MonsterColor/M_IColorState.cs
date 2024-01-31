using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface M_IColorState
{
    public float M_JumpForce { get; }

    public int M_damage { get; }

    public int M_health { get; }

    public void Attack(MonsterController monster);
}
