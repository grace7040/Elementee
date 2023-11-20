using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }
    public bool WallSliding { get { return false; } }


    //Temporal Setting : Green Color Attack -> Throw leaf obj
    public void Attack(PlayerController player)
    {
        //if (player.canAttack)
        {
            player.canAttack = false;
            GameObject throwableWeapon = Instantiate(Resources.Load("GreenWeapon"),
                player.transform.position + new Vector3(player.transform.localScale.x * 0.5f, 0.2f),
                Quaternion.identity) as GameObject;

            player.animator.SetBool("IsGreenAttacking", true);
            throwableWeapon.GetComponent<SpriteRenderer>().sprite = player.colorWeapons[(int)Colors.green].sprite;
            //throwableWeapon.GetComponent<SpriteRenderer>().sprite = this.Sprite;
            Vector2 direction = new Vector2(player.transform.localScale.x, 0);
            throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
            throwableWeapon.name = "GreenWeapon";

            player.CallOnDelay(0.5f, () =>
            {
                player.canAttack = true;
            });
        }
    }
}
