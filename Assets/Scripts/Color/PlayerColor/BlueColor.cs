using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 15; } }
    public bool WallSliding { get { return false; } }

    Sprite weaponSprite = null;
    bool isInit = false;

    //Temporal Setting : BLue Color Attack -> Throw Water obj
    public void Attack(PlayerController player)
    {

        player.canAttack = false;
        GameObject throwableWeapon = Instantiate(Resources.Load("Player/BlueWeapon"),
            player.transform.position + new Vector3(player.transform.localScale.x * 0.5f, 0.2f),
            Quaternion.identity) as GameObject;

        player.animator.SetBool("IsBlueAttacking", true);

        if (!isInit)
        {
            weaponSprite = player.colorWeapons[(int)Colors.blue].sprite;
            isInit = true;
        }

        throwableWeapon.GetComponent<SpriteRenderer>().sprite = weaponSprite;

        throwableWeapon.transform.localScale = new Vector3(throwableWeapon.transform.localScale.x * player.transform.localScale.normalized.x, throwableWeapon.transform.localScale.y, throwableWeapon.transform.localScale.z);
        AudioManager.Instacne.PlaySFX("Blue");
        //throwableWeapon.GetComponent<SpriteRenderer>().sprite = this.Sprite;
        Vector2 direction = new Vector2(player.transform.localScale.x, 0);
        throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
        throwableWeapon.name = "BlueWeapon";

        player.CallOnDelay(1f, () =>
        {
            player.canAttack = true;
        });
        

    }
}
