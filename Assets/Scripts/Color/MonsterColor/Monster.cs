using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster", order = int.MaxValue)]
public class Monster : ScriptableObject
{
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    [SerializeField]

    private int health;
    public int Health { get { return health; } }

    [SerializeField]
    private Colors myColor;
    public Colors MyColor { get { return myColor; } }
}
