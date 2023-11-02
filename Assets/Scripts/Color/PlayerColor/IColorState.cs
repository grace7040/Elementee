using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorState
{
    public float JumpForce { get; }
    public int Damage { get; }
    public bool WallSliding { get; }
    public void Attack(PlayerController player);
    public GameObject ThrowableObject { get; set; }


    public GameObject CustomObject { get; set; }

    public Sprite Sprite { get; set; }

}

