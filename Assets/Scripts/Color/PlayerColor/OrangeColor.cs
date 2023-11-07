using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return false; } }

    //�� ��ǳ
    public void Attack(PlayerController player)
    {

        //playerprefab on
        //player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(true);
        player.animator.SetBool("IsOrangeAttacking", true);

        //4�� ���Ŀ� off
        //Animation �� �Ծ� �� �׷���?
        //player.CallOnDelay(4f, () =>
        //{
        //    player.animator.SetBool("IsOrangeAttacking", false);
        //    player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(false);
        //}
        //);

    }
}
