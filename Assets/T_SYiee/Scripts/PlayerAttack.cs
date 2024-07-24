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

    [Header("AttackEffect")]
    public GameObject YellowAttackEffect;
    public GameObject OrangeAttackEffect;
    public GameObject PurpleAttackEffect;

    //Attack
    [HideInInspector] public bool canAttack = true;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        ColorManager.Instance.InitPlayerAttack(this, OnOrangeAttacked, OnYellowAttacked);
    }


    public void AttackDown()
    {
        canAttack = false;
        playerController.Color.Attack(transform.position, transform.localScale.x);
        this.CallOnDelay(playerController.Color.CoolTime, () => { canAttack = true; });   // ::TODO:: 노랑일 경우 예외처리 해야함
    }

    public void AttackUp()
    {
        //isAttack = false;
    }

    public void SetCustomWeapon()
    {
        print("");
        ColorWeapons[(int)playerController.myColor].sprite = DrawManager.Instance.sprites[(int)playerController.myColor];

        // Yellow 경우, 자식들에도 sprite 할당이 필요함
        if (playerController.myColor == Colors.Yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                YellowAttackEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
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
                YellowAttackEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
            }
        }
    }

    public void OnPurpleAttacked()
    {
        StartCoroutine(PurpleAttackEffectCo());
    }

    IEnumerator PurpleAttackEffectCo()
    {
        PurpleAttackEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PurpleAttackEffect.SetActive(false);
    }

    void OnOrangeAttacked(float durationTime)
    {
        gameObject.layer = (int)Layer.OrangeAttack; // layer 변경으로 충돌 처리 막음

        OrangeAttackEffect.SetActive(true);

        var originalMoveSpeed = playerController.MoveSpeed;
        playerController.MoveSpeed = 20f;

        this.CallOnDelay(durationTime, () =>
        {
            OrangeAttackEffect.SetActive(false);
            gameObject.layer = (int)Layer.Player;
            playerController.MoveSpeed = originalMoveSpeed;
        });
    }

    void OnYellowAttacked()
    {
        YellowAttackEffect.SetActive(true);
        YellowAttackEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
    }
}