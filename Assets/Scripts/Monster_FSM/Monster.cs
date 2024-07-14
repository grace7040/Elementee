using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster", order = int.MaxValue)]
public class Monster : ScriptableObject
{
    [SerializeField]
    private int _damage;
    public int Damage { get { return _damage; } }

    [SerializeField]
    private int _health;
    public int Health { get { return _health; } }

    [SerializeField]
    private float _attackRange;
    public float AttackRange { get { return _attackRange; } }

    [SerializeField]
    private Colors _myColor;
    public Colors MyColor { get { return _myColor; } }
}