using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Weapon")]
    public SpriteRenderer[] ColorWeapons;
    public GameObject RedWeapon;
    public GameObject OrangeWeapon;
    public GameObject PurpleWeapon;
    public GameObject GreenWeapon;
    public GameObject BlueWeapon;
    public GameObject BlackWeapon;
    public GameObject YellowWeaponEffect;
    public GameObject OrangeWeaponEffect;

    //Attack
    [HideInInspector] public bool canAttack = true;

    private PlayerController playerController;



    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }


    public void AttackDown()
    {
        canAttack = false;
        playerController.Color.Attack(transform.position, transform.localScale.x);
        this.CallOnDelay(playerController.Color.CoolTime, () => { canAttack = true; });   // ::TODO:: ����� ��� ����ó�� �ؾ���
    }

    public void AttackUp()
    {
        //isAttack = false;
    }

    public void SetCustomWeapon()
    {
        print("");
        ColorWeapons[(int)playerController.myColor].sprite = DrawManager.Instance.sprites[(int)playerController.myColor];

        // Yellow ���, �ڽĵ鿡�� sprite �Ҵ��� �ʿ���
        if (playerController.myColor == Colors.Yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                YellowWeaponEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
            }
        }
    }

    public void SetBasicWeapon()
    {
        ColorWeapons[(int)playerController.myColor].sprite = DrawManager.Instance.Basic_Sprites[(int)playerController.myColor];

        if (playerController.myColor == Colors.Yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                YellowWeaponEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
            }
        }
    }

}
