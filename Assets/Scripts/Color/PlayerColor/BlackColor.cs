using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    public void Attack(PlayerController player)
    {
        player.CallOnDelay(CoolTime, () => { player.canAttack = true; });

        if (player.isHoldingEnemy)
        {
            player.BlackThrow();
            player.canAttack = false;
            AudioManager.Instacne.PlaySFX("BlackRelease");
        }
        else
        {
            player.BlackPull();
            AudioManager.Instacne.PlaySFX("Black");
        }
    }

    public void SetPlayerColor(Colors mon_color)
    {
        switch (mon_color)
        {
            case Colors.def:
                break;
            case Colors.red:
                ColorManager.Instance.HasRed = true;
                break;
            case Colors.blue:
                ColorManager.Instance.HasBlue = true;
                break;
            case Colors.yellow:
                ColorManager.Instance.HasYellow = true;
                break;
        }
    }

    //private IEnumerator PullCoroutine()
    //{
    //    if (!isHoldingEnemy)
    //    {
    //        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //        float closestDistance = 7.5f;
    //        Transform closestEnemy = null;

    //        foreach (GameObject enemy in enemies)
    //        {
    //            float distance = Vector2.Distance(transform.position, enemy.transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                closestEnemy = enemy.transform;
    //            }
    //        }

    //        if (closestEnemy != null)
    //        {
    //            heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
    //            Enemy = closestEnemy.gameObject;

    //            //closestEnemy.GetComponent<Animator>().enabled = false;
    //            closestEnemy.GetComponent<MonsterController>().enabled = false;
    //            //closestEnemy.GetComponent<MonsterController>().isDie = true;
    //            //heldEnemyRigidbody.isKinematic = true;
    //            //heldEnemyRigidbody.simulated = false;
    //            closestEnemy.GetComponent<Rigidbody2D>().mass = 0.0f;
    //            closestEnemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    //            closestEnemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
    //            //closestEnemy.AddComponent<BloodEffect>();

    //            float distance = Vector2.Distance(Enemy.transform.position, transform.position);

    //            while (distance > 1f)
    //            {
    //                distance = Vector2.Distance(Enemy.transform.position, transform.position);
    //                Vector2 throwDirection = (transform.position - Enemy.transform.position).normalized;
    //                // heldEnemyRigidbody.AddForce(throwDirection * pullForce, ForceMode2D.Impulse);
    //                Enemy.transform.Translate(throwDirection * pullForce * Time.deltaTime);

    //                yield return null;
    //            }
    //            isHoldingEnemy = true;

    //        }
    //    }
    //    yield return new WaitForSeconds(0f);
    //}

    //public void BlackThrow()
    //{
    //    Rigidbody2D rb = Enemy.AddComponent<Rigidbody2D>();

    //    if (rb != null)
    //    {
    //        // 하위 객체의 Transform 얻기
    //        Transform childTransform = Enemy.gameObject.transform;

    //        // 부모-자식 관계 해제
    //        childTransform.SetParent(null);

    //        isHoldingEnemy = false;
    //        Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

    //        rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

    //        Vector2 throwDirection = (rb.transform.position - transform.position).normalized;
    //        rb.velocity = throwDirection * throwForce;
    //        rb.gameObject.tag = "WeaponB"; // 태그 변경
    //        heldEnemyRigidbody = null;

    //        // 죽은 몬스터 색 획득
    //        Enemy.GetComponent<MonsterController>().Die();
    //    }
    //}
}
