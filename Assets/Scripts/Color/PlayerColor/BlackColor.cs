using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    #region variables
    public float pullForce = 2.0f; // 끌어당기는 힘 조절용 변수
    public float throwForce = 15f; // 던지는 힘 조절용 변수
    public bool isHoldingEnemy = false; // 적을 가지고 있는지 여부
    private Rigidbody2D heldEnemyRigidbody; // 가지고 있는 적의 Rigidbody2D
    private Transform playerTransform; // 플레이어의 Transform
    private GameObject Enemy;
    #endregion

    public void Attack(PlayerController player)
    {
        player.CallOnDelay(CoolTime, () => { player.canAttack = true; });

        foreach (Transform child in player.transform)
        {
            if (child.name == "WeaponPosition")
            {
                foreach (Transform child_ in child.transform)
                {
                    if (child_.name.Contains("M_"))
                    {
                        isHoldingEnemy = true;
                    }
                }
            }
        }

        if (isHoldingEnemy)
        {
            ThrowHeldEnemy();
            player.canAttack = false;
            AudioManager.Instacne.PlaySFX("BlackRelease");
        }
        else
        {
            playerTransform = player.transform;
            PullClosestEnemy(playerTransform);
            AudioManager.Instacne.PlaySFX("Black");
        }
    }

    // 가장 가까운 적을 끌어당김
    private void PullClosestEnemy(Transform playerTransform)
    {
        if (!isHoldingEnemy)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = 7.5f;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestEnemy != null)
            {
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
                Enemy = closestEnemy.gameObject;

                closestEnemy.GetComponent<Animator>().enabled = false;
                closestEnemy.GetComponent<MonsterController>().enabled = false;
                closestEnemy.GetComponent<Rigidbody2D>().mass = 0.1f;
                closestEnemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                closestEnemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
                closestEnemy.AddComponent<BloodEffect>();               

                Vector2 throwDirection = (playerTransform.position - heldEnemyRigidbody.transform.position).normalized;
                heldEnemyRigidbody.AddForce(throwDirection * pullForce, ForceMode2D.Impulse);
            }
        }
    }

    // 가지고 있는 적을 던짐
    private void ThrowHeldEnemy()
    {
        Rigidbody2D rb = Enemy.AddComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 하위 객체의 Transform 얻기
            Transform childTransform = Enemy.gameObject.transform;

            // 부모-자식 관계 해제
            childTransform.SetParent(null);

            isHoldingEnemy = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - playerTransform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB"; // 태그 변경
            heldEnemyRigidbody = null;

            // 죽은 몬스터 색 획득
            SetPlayerColor(Enemy.GetComponent<MonsterController>().myColor);

            Destroy(Enemy.gameObject, 2.0f);
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
}
