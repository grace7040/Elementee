using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }

    //Temporal Setting : Red Color Attack -> Throw obj
    public void Attack(PlayerController player)
    {
        GameObject throwableWeapon = Instantiate(player.throwableObject, 
            player.transform.position + new Vector3(player.transform.localScale.x * 0.5f, -0.2f), 
            Quaternion.identity) as GameObject;
        Vector2 direction = new Vector2(player.transform.localScale.x, 0);
        throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
        throwableWeapon.name = "ThrowableWeapon";
    }
}
