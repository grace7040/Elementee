using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }

    public GameObject throwableObject { get; set; }
    public Sprite sprite { get; set; }

    //Temporal Setting : BLue Color Attack -> Throw Water obj
    public void Attack(PlayerController player)
    {
        GameObject throwableWeapon = Instantiate(Resources.Load("BlueWeapon"), 
            player.transform.position + new Vector3(player.transform.localScale.x * 0.5f, -0.2f), 
            Quaternion.identity) as GameObject;

        //throwableWeapon.GetComponent<SpriteRenderer>().sprite = this.sprite;
        Vector2 direction = new Vector2(player.transform.localScale.x, 0);
        throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
        throwableWeapon.name = "BlueWeapon";
    }
}
