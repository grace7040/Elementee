using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrangeColor : IColorState
{
    public int Damage { get { return 25; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 6f; } }

    public float durationTime = 5f;
    //�� ��ǳ
    public void Attack(PlayerController player)
    {
        //player.canAttack = false;
        player.gameObject.layer = 10; // layer �������� �浹 ó�� ����
        AudioManager.Instacne.PlaySFX("Orange");

        // �׸� ���� + effect �ѱ�
        player.orange_WeaponEffect.SetActive(true);
        //player.orange_Weapon.SetActive(true);

        //�̵��ӵ� up
        player.m_MoveSpeed = 20f;
     

        // ���ӽð�
        player.CallOnDelay(durationTime, () =>
        {
            player.orange_WeaponEffect.SetActive(false);
            player.gameObject.layer = 3;
            player.m_MoveSpeed = 10f;

            //�̵����� down
        });


        //��Ÿ�� 
        player.CallOnDelay(CoolTime, () =>
        {
            player.canAttack = true;
        });


        //player.orange_WeaponEffect.GetComponent<SpriteRenderer>().DOFlip();
        //player.animator.SetBool("IsOrangeAttacking", true);
    }


}
