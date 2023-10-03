using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }

    public GameObject throwableObject { get; set; }
    public Sprite sprite { get; set; }

    //Temporal Setting : Yellow Color Attack -> ���� ����
    public void Attack(PlayerController player)
    {
        //���� 
        //playerprefab on
        player.GetComponent<PlayerController>().yellowAttackEffect.SetActive(true);
        //Ư�� �ð� ���Ŀ� off

    }
}
