using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return false; } }

    //¸ð·¡ ÆøÇ³
    public void Attack(PlayerController player)
    {

        //playerprefab on
        //player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(true);
        player.animator.SetBool("IsOrangeAttacking", true);

        //4ÃÊ ÀÌÈÄ¿¡ off
        //Animation ¾È ¸Ô¾î ¿Ö ±×·²±î?
        //player.CallOnDelay(4f, () =>
        //{
        //    player.animator.SetBool("IsOrangeAttacking", false);
        //    player.GetComponent<PlayerController>().orangeAttackEffect.SetActive(false);
        //}
        //);

    }
}
