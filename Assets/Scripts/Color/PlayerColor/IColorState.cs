using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorState
{
    public float JumpForce { get; }
    public int Damage { get; }
    public void Attack(PlayerController player);
    public GameObject throwableObject { get; set; }
    public Sprite sprite { get; set; }

}

