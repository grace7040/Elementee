using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface M_IColorState
{
    public float JumpForce { get; }
    public void Attack(MonsterController monster);
}
